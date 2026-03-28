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
const searchParams = new URLSearchParams(window.location.search);
const reservationId = Number.parseInt(searchParams.get('id') ?? '', 10);

function showError(message) {
  if (!alertElement) {
    return;
  }

  alertElement.textContent = message;
  alertElement.classList.remove('d-none');
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
}

if (!Number.isInteger(reservationId) || reservationId <= 0) {
  showError('Missing or invalid reservation id in URL.');
} else {
  const detailUrl = `${config.apiUrl}/api/reservation/get-reservation/${reservationId}`;

  fetch(detailUrl)
    .then(response => {
      if (!response.ok) {
        throw new Error(`Request failed with status ${response.status}`);
      }

      return response.json();
    })
    .then(reservation => {
      if (!reservation) {
        showError('Reservation not found.');
        return;
      }

      populateReservation(reservation);
    })
    .catch(error => {
      console.error('Failed to fetch reservation detail:', error);
      showError('Failed to load reservation details.');
    });
}
