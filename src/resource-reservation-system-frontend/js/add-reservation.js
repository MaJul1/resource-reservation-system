import { config } from './config.js';

const titleElement = document.getElementById('add-reservation-title');
const alertElement = document.getElementById('add-reservation-alert');
const formElement = document.getElementById('add-reservation-form');
const saveButtonElement = document.getElementById('saveButton');
const cancelButtonElement = document.getElementById('cancelButton');

const startDateInput = document.getElementById('startDateInput');
const startTimeInput = document.getElementById('startTimeInput');
const endDateInput = document.getElementById('endDateInput');
const endTimeInput = document.getElementById('endTimeInput');
const userFirstNameInput = document.getElementById('userFirstName');
const userLastNameInput = document.getElementById('userLastName');
const userEmailInput = document.getElementById('userEmail');
const userPhoneInput = document.getElementById('userPhone');

const params = new URLSearchParams(window.location.search);
const resourceId = Number.parseInt(params.get('id') ?? '', 10);

function redirectTo404() {
  window.location.href = '404.html';
}

function showAlert(message, type) {
  if (!alertElement) {
    return;
  }

  alertElement.textContent = message;
  alertElement.className = `alert alert-${type}`;
}

function clearAlert() {
  if (!alertElement) {
    return;
  }

  alertElement.textContent = '';
  alertElement.className = 'alert d-none';
}

function toDateValue(date) {
  const pad = value => String(value).padStart(2, '0');
  return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}`;
}

function toTimeValue(date) {
  const pad = value => String(value).padStart(2, '0');
  return `${pad(date.getHours())}:${pad(date.getMinutes())}`;
}

function setDefaultDateTimeInputs() {
  if (!startDateInput || !startTimeInput || !endDateInput || !endTimeInput) {
    return;
  }

  if (startDateInput.value || startTimeInput.value || endDateInput.value || endTimeInput.value) {
    return;
  }

  const now = new Date();
  const nextHour = new Date(now);
  nextHour.setMinutes(0, 0, 0);
  nextHour.setHours(nextHour.getHours() + 1);

  const end = new Date(nextHour);
  end.setHours(end.getHours() + 1);

  startDateInput.value = toDateValue(nextHour);
  startTimeInput.value = toTimeValue(nextHour);
  endDateInput.value = toDateValue(end);
  endTimeInput.value = toTimeValue(end);
}

function getCombinedDate(dateInput, timeInput) {
  if (!dateInput?.value || !timeInput?.value) {
    return null;
  }

  const combinedDate = new Date(`${dateInput.value}T${timeInput.value}`);
  if (Number.isNaN(combinedDate.getTime())) {
    return null;
  }

  return combinedDate;
}

function validateForm() {
  if (!formElement) {
    return { isValid: false, errorMessage: 'Form is not ready.' };
  }

  const firstName = userFirstNameInput?.value.trim() ?? '';
  const lastName = userLastNameInput?.value.trim() ?? '';
  const email = userEmailInput?.value.trim() ?? '';
  const phoneNumber = userPhoneInput?.value.trim() ?? '';

  if (!firstName || !lastName || !email || !phoneNumber) {
    return { isValid: false, errorMessage: 'Please fill out all required fields.' };
  }

  if (firstName.length < 2 || lastName.length < 2) {
    return { isValid: false, errorMessage: 'First name and last name must be at least 2 characters.' };
  }

  const phonePattern = /^\+?[0-9]{7,15}$/;
  if (!phonePattern.test(phoneNumber)) {
    return { isValid: false, errorMessage: 'Phone number must contain only digits and optionally leading + (7-15 digits).' };
  }

  const start = getCombinedDate(startDateInput, startTimeInput);
  const end = getCombinedDate(endDateInput, endTimeInput);

  if (!start || !end) {
    return { isValid: false, errorMessage: 'Please provide valid start and end date/time.' };
  }

  if (start >= end) {
    return { isValid: false, errorMessage: 'Start date/time must be earlier than end date/time.' };
  }

  const durationInMinutes = (end.getTime() - start.getTime()) / 60000;
  if (durationInMinutes < 60) {
    return { isValid: false, errorMessage: 'Reservation duration must be at least 60 minutes.' };
  }

  return {
    isValid: true,
    payload: {
      start: start.toISOString(),
      end: end.toISOString(),
      resourceId,
      firstName,
      lastName,
      email,
      phoneNumber
    }
  };
}

async function submitReservation(payload) {
  const response = await fetch(`${config.apiUrl}/api/reservation/create-reservation`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(payload)
  });

  if (!response.ok) {
    let message = `Request failed with status ${response.status}`;

    try {
      const errorPayload = await response.json();
      if (errorPayload?.message) {
        message = errorPayload.message;
      }
    } catch {
      message = `Request failed with status ${response.status}`;
    }

    throw new Error(message);
  }
}

async function handleFormSubmit(event) {
  event.preventDefault();
  clearAlert();

  if (!formElement) {
    return;
  }

  if (!formElement.checkValidity()) {
    formElement.reportValidity();
    return;
  }

  const validationResult = validateForm();

  if (!validationResult.isValid) {
    showAlert(validationResult.errorMessage, 'danger');
    return;
  }

  if (saveButtonElement) {
    saveButtonElement.disabled = true;
  }

  try {
    await submitReservation(validationResult.payload);
    window.location.href = 'reservation.html';
  } catch (error) {
    showAlert(error.message ?? 'Failed to create reservation.', 'danger');
  } finally {
    if (saveButtonElement) {
      saveButtonElement.disabled = false;
    }
  }
}

function setTitle(name) {
  if (!titleElement) {
    return;
  }

  const resourceName = name?.trim();

  if (!resourceName) {
    redirectTo404();
    return;
  }

  titleElement.textContent = `Add New ${resourceName} Reservation`;
  document.title = `Add Reservation - ${resourceName}`;
}

if (!Number.isInteger(resourceId) || resourceId <= 0) {
  redirectTo404();
} else {
  const resourceUrl = `${config.apiUrl}/api/resource/get-resrouce-by-id/${resourceId}`;

  fetch(resourceUrl)
    .then(response => {
      if (!response.ok) {
        throw new Error(`Request failed with status ${response.status}`);
      }

      return response.json();
    })
    .then(resource => {
      if (!resource || !resource.name) {
        redirectTo404();
        return;
      }

      setTitle(resource.name);

      if (cancelButtonElement) {
        cancelButtonElement.href = 'reservation.html';
      }

      setDefaultDateTimeInputs();

      if (formElement) {
        formElement.addEventListener('submit', handleFormSubmit);
      }
    })
    .catch(() => {
      redirectTo404();
    });
}
