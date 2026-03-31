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
  resourceId: document.getElementById('resource-id'),
  resourceName: document.getElementById('resource-name'),
  resourceType: document.getElementById('resource-type'),
  resourceDescription: document.getElementById('resource-description')
};

const alertElement = document.getElementById('resource-detail-alert');
const tableBody = document.getElementById('resource-reservation-table-body');
const addReservationLinkElement = document.getElementById('add-reservation-link');
const paginationElement = document.getElementById('resource-detail-pagination');
const pageSizeElement = document.getElementById('resource-detail-page-size');

const searchParams = new URLSearchParams(window.location.search);
const resourceId = Number.parseInt(searchParams.get('id') ?? '', 10);
const currentPage = Math.max(Number.parseInt(searchParams.get('page') ?? '1', 10) || 1, 1);
const pageSize = Math.max(Number.parseInt(searchParams.get('size') ?? '10', 10) || 10, 1);
const sortBy = searchParams.get('sortBy') ?? 'id';

if (addReservationLinkElement && Number.isInteger(resourceId) && resourceId > 0) {
  const params = new URLSearchParams();
  params.set('id', String(resourceId));
  addReservationLinkElement.href = `add-reservation.html?${params.toString()}`;
}

function redirectTo404() {
  window.location.href = '404.html';
}

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

function setMessageRow(message) {
  if (!tableBody) {
    return;
  }

  tableBody.innerHTML = `<tr><td colspan="5" class="text-center">${message}</td></tr>`;
}

function buildPageUrl(page) {
  const params = new URLSearchParams(window.location.search);
  params.set('page', String(Math.max(page, 1)));
  params.set('size', String(pageSize));
  params.set('sortBy', sortBy);

  return `${window.location.pathname}?${params.toString()}`;
}

function renderPagination(itemCount) {
  if (!paginationElement) {
    return;
  }

  const isPreviousDisabled = currentPage <= 1;
  const isNextDisabled = itemCount < pageSize;

  paginationElement.innerHTML = `
    <li class="page-item ${isPreviousDisabled ? 'disabled' : ''}">
      <a class="page-link" href="${isPreviousDisabled ? '#' : buildPageUrl(currentPage - 1)}">Previous</a>
    </li>
    <li class="page-item active" aria-current="page">
      <span class="page-link">${currentPage}</span>
    </li>
    <li class="page-item ${isNextDisabled ? 'disabled' : ''}">
      <a class="page-link" href="${isNextDisabled ? '#' : buildPageUrl(currentPage + 1)}">Next</a>
    </li>
  `;
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

function getUserFullName(user) {
  const firstName = user?.firstName ?? '';
  const lastName = user?.lastName ?? '';
  const fullName = `${firstName} ${lastName}`.trim();

  return fullName || '-';
}

function populateResource(resource) {
  setValue(fields.resourceId, String(resource.id ?? '-'));
  setValue(fields.resourceName, resource.name ?? '-');
  setValue(fields.resourceType, resource.type ?? '-');
  setValue(fields.resourceDescription, resource.description ?? '-');
}

function renderReservations(reservations) {
  if (!tableBody) {
    return;
  }

  if (!Array.isArray(reservations) || reservations.length === 0) {
    setMessageRow('No reservations found for this resource.');
    renderPagination(0);
    return;
  }

  tableBody.innerHTML = '';

  reservations.forEach(reservation => {
    const row = document.createElement('tr');
    row.style.cursor = 'pointer';

    row.addEventListener('click', () => {
      const params = new URLSearchParams(window.location.search);
      params.set('id', String(reservation.id));
      window.location.href = `detailed-reservation.html?${params.toString()}`;
    });

    row.innerHTML = `
      <td>${reservation.id ?? '-'}</td>
      <td>${getUserFullName(reservation.user)}</td>
      <td>${formatDate(reservation.start)}</td>
      <td>${formatDate(reservation.end)}</td>
      <td>${getStatusLabel(reservation.status)}</td>
    `;

    tableBody.appendChild(row);
  });

  renderPagination(reservations.length);
}

async function loadResource() {
  const resourceUrl = `${config.apiUrl}/api/resource/get-resrouce-by-id/${resourceId}`;
  const response = await fetch(resourceUrl);

  if (response.status === 404) {
    redirectTo404();
    return;
  }

  if (!response.ok) {
    throw new Error(`Request failed with status ${response.status}`);
  }

  const resource = await response.json();

  if (!resource) {
    redirectTo404();
    return;
  }

  populateResource(resource);
}

async function loadReservations() {
  const reservationUrl = new URL(`${config.apiUrl}/api/reservation/get-reservation-by-resource/${resourceId}`);
  reservationUrl.searchParams.set('page', String(currentPage));
  reservationUrl.searchParams.set('size', String(pageSize));
  reservationUrl.searchParams.set('sortBy', sortBy);

  const response = await fetch(reservationUrl.toString());

  if (!response.ok) {
    throw new Error(`Request failed with status ${response.status}`);
  }

  const reservations = await response.json();
  renderReservations(reservations);
}

if (pageSizeElement) {
  pageSizeElement.value = String(pageSize);
  pageSizeElement.addEventListener('change', event => {
    const newSize = Number.parseInt(event.target.value, 10) || 10;
    const params = new URLSearchParams(window.location.search);
    params.set('page', '1');
    params.set('size', String(newSize));
    params.set('sortBy', sortBy);
    window.location.search = params.toString();
  });
}

if (!Number.isInteger(resourceId) || resourceId <= 0) {
  showError('Missing or invalid resource id in URL.');
  setMessageRow('Unable to load reservations.');
  renderPagination(0);
} else {
  Promise.all([loadResource(), loadReservations()]).catch(error => {
    console.error('Failed to fetch resource detail:', error);
    showError('Failed to load resource details.');
    setMessageRow('Failed to load reservations.');
    renderPagination(0);
  });
}
