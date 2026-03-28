import { config } from './config.js';

const getResourcePathUrl = config.apiUrl + '/api/resource/get-resources';
const tableBody = document.getElementById('resource-table-body');
const paginationElement = document.getElementById('resource-pagination');
const pageSizeElement = document.getElementById('resource-page-size');

const searchParams = new URLSearchParams(window.location.search);
const currentPage = Math.max(Number.parseInt(searchParams.get('page') ?? '1', 10) || 1, 1);
const pageSize = Math.max(Number.parseInt(searchParams.get('size') ?? '10', 10) || 10, 1);
const sortBy = searchParams.get('sortBy') ?? 'id';

function setMessageRow(message) {
  if (!tableBody) {
    return;
  }

  tableBody.innerHTML = `<tr><td colspan="4" class="text-center">${message}</td></tr>`;
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

function renderResources(resources) {
  if (!tableBody) {
    return;
  }

  if (!Array.isArray(resources) || resources.length === 0) {
    setMessageRow('No resources found.');
    renderPagination(0);
    return;
  }

  tableBody.innerHTML = '';

  resources.forEach(resource => {
    const row = document.createElement('tr');

    row.innerHTML = `
      <td>${resource.id ?? '-'}</td>
      <td>${resource.name ?? '-'}</td>
      <td>${resource.type ?? '-'}</td>
      <td>${resource.description ?? '-'}</td>
    `;

    tableBody.appendChild(row);
  });

  renderPagination(resources.length);
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

const apiUrl = new URL(getResourcePathUrl);
apiUrl.searchParams.set('page', String(currentPage));
apiUrl.searchParams.set('size', String(pageSize));
apiUrl.searchParams.set('sortBy', sortBy);

fetch(apiUrl.toString())
  .then(response => {
    if (!response.ok) {
      throw new Error(`Request failed with status ${response.status}`);
    }

    return response.json();
  })
  .then(resources => {
    renderResources(resources);
  })
  .catch(error => {
    console.error('Failed to fetch resources:', error);
    setMessageRow('Failed to load resources.');
    renderPagination(0);
  });
