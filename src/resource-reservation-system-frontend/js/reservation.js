import { config } from './config.js';

const getReservationPathUrl = config.apiUrl + "/api/reservation/get-reservations";

fetch(getReservationPathUrl)
  .then(r => {
    return r.json();
  }).then (r => {
    console.log(r);
  });