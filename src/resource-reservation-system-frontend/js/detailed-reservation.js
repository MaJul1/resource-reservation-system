import { config } from './config.js';

const statusLabels = {
  1: 'Pending',
  2: 'Approved',
  3: 'Denied',
  4: 'Cancelled',
  5: 'Ongoing',
  6: 'Done'
};

const fields = {
  reservationId: document.getElementById('reservation-id'),
  reservationStatus: document.getElementById('reservation-status'),
  reservationStart: document.getElementById('reservation-start'),
  reservationEnd: document.getElementById('reservation-end'),
  resourceId: document.getElementById('resource-id'),
  resourceName: document.getElementById('resource-name'),
  resourceType: document.getElementById('resource-type'),
  resourceDescription: document.getElementById('resource-description'),
  userFirstName: document.getElementById('user-first-name'),
  userLastName: document.getElementById('user-last-name'),
  userPhoneNumber: document.getElementById('user-phone-number'),
  userEmail: document.getElementById('user-email')
};

const alertElement = document.getElementById('reservation-detail-alert');
const actionsElement = document.getElementById('reservation-actions');
const searchParams = new URLSearchParams(window.location.search);
const reservationId = Number.parseInt(searchParams.get('id') ?? '', 10);
let currentReservation = null;

function showError(message) {
  if (!alertElement) {
    return;
  }

  alertElement.textContent = message;
  alertElement.classList.remove('alert-success');
  alertElement.classList.add('alert-danger');
  alertElement.classList.remove('d-none');
}

function showSuccess(message) {
  if (!alertElement) {
    return;
  }

  alertElement.textContent = message;
  alertElement.classList.remove('alert-danger');
  alertElement.classList.add('alert-success');
  alertElement.classList.remove('d-none');
}

function clearAlert() {
  if (!alertElement) {
    return;
  }

  alertElement.textContent = '';
  alertElement.classList.add('d-none');
}

function setValue(element, value) {
  if (!element) {
    return;
  }

  element.value = value ?? '-';
}

function formatDate(dateValue) {
  const date = new Date(dateValue);

  if (Number.isNaN(date.getTime())) {
    return dateValue ?? '-';
  }

  return date.toLocaleString(undefined, {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: 'numeric',
    minute: '2-digit'
  });
}

function getStatusLabel(status) {
  if (typeof status === 'number') {
    return statusLabels[status] ?? String(status);
  }

  if (typeof status === 'string') {
    return status;
  }

  return '-';
}

function toDateTimeLocalValue(dateValue) {
  const date = new Date(dateValue);

  if (Number.isNaN(date.getTime())) {
    return '';
  }

  const pad = value => String(value).padStart(2, '0');
  const year = date.getFullYear();
  const month = pad(date.getMonth() + 1);
  const day = pad(date.getDate());
  const hours = pad(date.getHours());
  const minutes = pad(date.getMinutes());

  return `${year}-${month}-${day}T${hours}:${minutes}`;
}

function buildBackUrl() {
  const params = new URLSearchParams(window.location.search);
  params.delete('id');
  const query = params.toString();

  return query ? `reservation.html?${query}` : 'reservation.html';
}

function createActionButton(label, className, onClick) {
  const button = document.createElement('button');
  button.type = 'button';
  button.className = className;
  button.textContent = label;
  button.addEventListener('click', onClick);

  return button;
}

function parseErrorMessage(payload, fallbackMessage) {
  if (payload && typeof payload === 'object' && typeof payload.message === 'string') {
    return payload.message;
  }

  return fallbackMessage;
}

async function postReservationAction(actionPath) {
  clearAlert();
  const url = `${config.apiUrl}/api/reservation/${actionPath}?id=${reservationId}`;
  const response = await fetch(url, { method: 'POST' });

  if (!response.ok) {
    let payload = null;

    try {
      payload = await response.json();
    } catch {
      payload = null;
    }

    throw new Error(parseErrorMessage(payload, `Action failed with status ${response.status}`));
  }
}

async function submitMoveAction() {
  clearAlert();

  const defaultStart = toDateTimeLocalValue(currentReservation?.start);
  const defaultEnd = toDateTimeLocalValue(currentReservation?.end);

  const newStartText = window.prompt('Enter new start (YYYY-MM-DDTHH:mm)', defaultStart);
  if (!newStartText) {
    return;
  }

  const newEndText = window.prompt('Enter new end (YYYY-MM-DDTHH:mm)', defaultEnd);
  if (!newEndText) {
    return;
  }

  const parsedStart = new Date(newStartText);
  const parsedEnd = new Date(newEndText);

  if (Number.isNaN(parsedStart.getTime()) || Number.isNaN(parsedEnd.getTime())) {
    showError('Invalid date input. Use YYYY-MM-DDTHH:mm format.');
    return;
  }

  const response = await fetch(`${config.apiUrl}/api/reservation/move-reservation`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      id: reservationId,
      newStart: parsedStart.toISOString(),
      newEnd: parsedEnd.toISOString()
    })
  });

  if (!response.ok) {
    let payload = null;

    try {
      payload = await response.json();
    } catch {
      payload = null;
    }

    throw new Error(parseErrorMessage(payload, `Move failed with status ${response.status}`));
  }
}

function renderActions(status) {
  if (!actionsElement) {
    return;
  }

  actionsElement.innerHTML = '';

  if (status === 1) {
    actionsElement.appendChild(createActionButton('Approve', 'btn btn-success', async () => {
      try {
        await postReservationAction('approve-reservation');
        showSuccess('Reservation approved.');
        await loadReservation();
      } catch (error) {
        showError(error.message);
      }
    }));

    actionsElement.appendChild(createActionButton('Move', 'btn btn-warning', async () => {
      try {
        await submitMoveAction();
        showSuccess('Reservation moved.');
        await loadReservation();
      } catch (error) {
        showError(error.message);
      }
    }));

    actionsElement.appendChild(createActionButton('Deny', 'btn btn-outline-danger', async () => {
      try {
        await postReservationAction('deny-reservation');
        showSuccess('Reservation denied.');
        await loadReservation();
      } catch (error) {
        showError(error.message);
      }
    }));

    actionsElement.appendChild(createActionButton('Cancel', 'btn btn-danger', async () => {
      try {
        await postReservationAction('cancel-reservation');
        showSuccess('Reservation cancelled.');
        await loadReservation();
      } catch (error) {
        showError(error.message);
      }
    }));
  }

  if (status === 2) {
    actionsElement.appendChild(createActionButton('Cancel', 'btn btn-danger', async () => {
      try {
        await postReservationAction('cancel-reservation');
        showSuccess('Reservation cancelled.');
        await loadReservation();
      } catch (error) {
        showError(error.message);
      }
    }));
  }

  const backButton = createActionButton('Back', 'btn btn-secondary', () => {
    window.location.href = buildBackUrl();
  });
  actionsElement.appendChild(backButton);
}

function populateReservation(reservation) {
  setValue(fields.reservationId, String(reservation.id ?? '-'));
  setValue(fields.reservationStatus, getStatusLabel(reservation.status));
  setValue(fields.reservationStart, formatDate(reservation.start));
  setValue(fields.reservationEnd, formatDate(reservation.end));

  setValue(fields.resourceId, String(reservation.resource?.id ?? '-'));
  setValue(fields.resourceName, reservation.resource?.name ?? '-');
  setValue(fields.resourceType, reservation.resource?.type ?? '-');
  setValue(fields.resourceDescription, reservation.resource?.description ?? '-');

  setValue(fields.userFirstName, reservation.user?.firstName ?? '-');
  setValue(fields.userLastName, reservation.user?.lastName ?? '-');
  setValue(fields.userPhoneNumber, reservation.user?.phoneNumber ?? '-');
  setValue(fields.userEmail, reservation.user?.email ?? '-');

  renderActions(reservation.status);
}

async function loadReservation() {
  const detailUrl = `${config.apiUrl}/api/reservation/get-reservation/${reservationId}`;

  const response = await fetch(detailUrl);

  if (!response.ok) {
    throw new Error(`Request failed with status ${response.status}`);
  }

  const reservation = await response.json();

  if (!reservation) {
    throw new Error('Reservation not found.');
  }

  currentReservation = reservation;
  populateReservation(reservation);
}

if (!Number.isInteger(reservationId) || reservationId <= 0) {
  showError('Missing or invalid reservation id in URL.');
  renderActions(undefined);
} else {
  loadReservation().catch(error => {
    console.error('Failed to fetch reservation detail:', error);
    showError('Failed to load reservation details.');
    renderActions(undefined);
  });
}
