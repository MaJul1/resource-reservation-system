import { config } from './config.js';

const getReservationPathUrl = config.apiUrl + "/api/reservation/get-reservations";
const getResourceInfoPathUrl = config.apiUrl + "/api/resource/get-resources-info";
const tableBody = document.getElementById("reservation-table-body");
const paginationElement = document.getElementById("reservation-pagination");
const pageSizeElement = document.getElementById("reservation-page-size");
const resourceSelectElement = document.getElementById("reservation-resource-select");
const addReservationLinkElement = document.getElementById("add-reservation-link");

const searchParams = new URLSearchParams(window.location.search);
const currentPage = Math.max(Number.parseInt(searchParams.get("page") ?? "1", 10) || 1, 1);
const pageSize = Math.max(Number.parseInt(searchParams.get("size") ?? "10", 10) || 10, 1);
const sortBy = searchParams.get("sortBy") ?? "id";

const statusLabels = {
  1: "Pending",
  2: "Approved",
  3: "Denied",
  4: "Cancelled",
  5: "Ongoing",
  6: "Done"
};

function formatDate(dateValue) {
  const date = new Date(dateValue);

  if (Number.isNaN(date.getTime())) {
    return dateValue ?? "-";
  }

  return date.toLocaleString(undefined, {
    year: "numeric",
    month: "long",
    day: "numeric",
    hour: "numeric",
    minute: "2-digit"
  });
}

function getStatusLabel(status) {
  if (typeof status === "number") {
    return statusLabels[status] ?? String(status);
  }

  if (typeof status === "string") {
    return status;
  }

  return "-";
}

function getUserFullName(user) {
  const firstName = user?.firstName ?? "";
  const lastName = user?.lastName ?? "";
  const fullName = `${firstName} ${lastName}`.trim();

  return fullName || "-";
}

function setMessageRow(message) {
  if (!tableBody) {
    return;
  }

  tableBody.innerHTML = `<tr><td colspan="6" class="text-center">${message}</td></tr>`;
}

function setResourceOptions(resources) {
  if (!resourceSelectElement) {
    return;
  }

  if (!Array.isArray(resources) || resources.length === 0) {
    resourceSelectElement.innerHTML = "<option selected>No resources found</option>";
    resourceSelectElement.disabled = true;
    return;
  }

  resourceSelectElement.disabled = false;
  resourceSelectElement.innerHTML =
    '<option value="" selected disabled>Select a resource</option>' +
    resources
      .map(resource => `<option value="${resource.id}">${resource.name ?? "-"}</option>`)
      .join("");
}

if (addReservationLinkElement) {
  addReservationLinkElement.addEventListener("click", event => {
    const selectedResourceId = resourceSelectElement?.value;

    if (!selectedResourceId) {
      event.preventDefault();
      window.alert("Please select a resource first.");
      return;
    }

    const params = new URLSearchParams();
    params.set("id", selectedResourceId);
    addReservationLinkElement.href = `add-reservation.html?${params.toString()}`;
  });
}

function buildPageUrl(page) {
  const params = new URLSearchParams(window.location.search);
  params.set("page", String(Math.max(page, 1)));
  params.set("size", String(pageSize));
  params.set("sortBy", sortBy);

  return `${window.location.pathname}?${params.toString()}`;
}

function renderPagination(itemCount) {
  if (!paginationElement) {
    return;
  }

  const isPreviousDisabled = currentPage <= 1;
  const isNextDisabled = itemCount < pageSize;

  paginationElement.innerHTML = `
    <li class="page-item ${isPreviousDisabled ? "disabled" : ""}">
      <a class="page-link" href="${isPreviousDisabled ? "#" : buildPageUrl(currentPage - 1)}">Previous</a>
    </li>
    <li class="page-item active" aria-current="page">
      <span class="page-link">${currentPage}</span>
    </li>
    <li class="page-item ${isNextDisabled ? "disabled" : ""}">
      <a class="page-link" href="${isNextDisabled ? "#" : buildPageUrl(currentPage + 1)}">Next</a>
    </li>
  `;
}

function renderReservations(reservations) {
  if (!tableBody) {
    return;
  }

  if (!Array.isArray(reservations) || reservations.length === 0) {
    setMessageRow("No reservations found.");
    renderPagination(0);
    return;
  }

  tableBody.innerHTML = "";

  reservations.forEach((reservation, index) => {
    const row = document.createElement("tr");
    row.style.cursor = "pointer";
    row.addEventListener("click", () => {
      const params = new URLSearchParams(window.location.search);
      params.set("id", String(reservation.id));
      window.location.href = `detailed-reservation.html?${params.toString()}`;
    });

    row.innerHTML = `
      <td>${(currentPage - 1) * pageSize + index + 1}</td>
      <td>${reservation.resource?.name ?? "-"}</td>
      <td>${getUserFullName(reservation.user)}</td>
      <td>${formatDate(reservation.start)}</td>
      <td>${formatDate(reservation.end)}</td>
      <td>${getStatusLabel(reservation.status)}</td>
    `;

    tableBody.appendChild(row);
  });

  renderPagination(reservations.length);
}

if (pageSizeElement) {
  pageSizeElement.value = String(pageSize);
  pageSizeElement.addEventListener("change", event => {
    const newSize = Number.parseInt(event.target.value, 10) || 10;
    const params = new URLSearchParams(window.location.search);
    params.set("page", "1");
    params.set("size", String(newSize));
    params.set("sortBy", sortBy);
    window.location.search = params.toString();
  });
}

const apiUrl = new URL(getReservationPathUrl);
apiUrl.searchParams.set("page", String(currentPage));
apiUrl.searchParams.set("size", String(pageSize));
apiUrl.searchParams.set("sortBy", sortBy);

fetch(getResourceInfoPathUrl)
  .then(response => {
    if (!response.ok) {
      throw new Error(`Request failed with status ${response.status}`);
    }

    return response.json();
  })
  .then(resources => {
    setResourceOptions(resources);
  })
  .catch(error => {
    console.error("Failed to fetch resources:", error);

    if (resourceSelectElement) {
      resourceSelectElement.innerHTML = "<option selected>Failed to load resources</option>";
      resourceSelectElement.disabled = true;
    }
  });

fetch(apiUrl.toString())
  .then(response => {
    if (!response.ok) {
      throw new Error(`Request failed with status ${response.status}`);
    }

    return response.json();
  })
  .then(reservations => {
    renderReservations(reservations);
  })
  .catch(error => {
    console.error("Failed to fetch reservations:", error);
    setMessageRow("Failed to load reservations.");
    renderPagination(0);
  });