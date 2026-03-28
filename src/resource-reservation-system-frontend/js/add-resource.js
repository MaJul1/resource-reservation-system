import { config } from './config.js';

const formElement = document.getElementById('add-resource-form');
const alertElement = document.getElementById('add-resource-alert');
const saveButtonElement = document.getElementById('saveButton');

const resourceNameInput = document.getElementById('resourceName');
const resourceTypeInput = document.getElementById('resourceType');
const resourceDescriptionInput = document.getElementById('resourceDescription');

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

function validateForm() {
  const name = resourceNameInput?.value.trim() ?? '';
  const type = resourceTypeInput?.value.trim() ?? '';
  const description = resourceDescriptionInput?.value.trim() ?? '';

  if (!name || !type || !description) {
    return { isValid: false, errorMessage: 'Please fill out all required fields.' };
  }

  if (name.length < 3) {
    return { isValid: false, errorMessage: 'Resource name must be at least 3 characters.' };
  }

  if (type.length < 3) {
    return { isValid: false, errorMessage: 'Resource type must be at least 3 characters.' };
  }

  if (description.length < 10) {
    return { isValid: false, errorMessage: 'Resource description must be at least 10 characters.' };
  }

  return {
    isValid: true,
    payload: {
      name,
      type,
      description
    }
  };
}

async function submitResource(payload) {
  const response = await fetch(`${config.apiUrl}/api/resource`, {
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

async function handleSubmit(event) {
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
    await submitResource(validationResult.payload);
    window.location.href = 'resource.html';
  } catch (error) {
    showAlert(error.message ?? 'Failed to create resource.', 'danger');
  } finally {
    if (saveButtonElement) {
      saveButtonElement.disabled = false;
    }
  }
}

if (formElement) {
  formElement.addEventListener('submit', handleSubmit);
}
