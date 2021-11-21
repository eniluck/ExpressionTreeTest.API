'use strict'

import DataTable from './DataTable/dataTable.js';
import ApiInteraction from './API/ApiInteraction.js';

let header = [
  {
    key : "id",
    caption : "Индентификатор"
  },
  {
    key : "name",
    caption : "Наименование"
  },
  {
    key : "releaseYear",
    caption : "Год выхода"
  },
  {
    key : "screenDiagonal",
    caption : "Диагональ экрана"
  },
  {
    key : "simCardCount",
    caption : "Количество сим карт"
  },
  {
    key : "simCardFormatId",
    caption : "Индентификатор формата сим карты"
  },
  {
    key : "simCardFormatName",
    caption : "Формат сим карты"
  },
  {
    key : "color",
    caption : "Цвет"
  },
]

let phones = [
  {
    id: 2,
    name: "Samsung Galaxy A72",
    releaseYear: 2021,
    screenDiagonal: 6.7,
    simCardCount: 2,
    simCardFormatId: 4,
    simCardFormatName: "Nano-SIM (4FF)",
    color: "лаванда"
  },
  {
    id: 3,
    name: "POCO X3 Pro",
    releaseYear: 2021,
    screenDiagonal: 6.67,
    simCardCount: 2,
    simCardFormatId: 4,
    simCardFormatName: "Nano-SIM (4FF)",
    color: "бежевый"
  }
];

let dataTable = new DataTable(phones, header);

let api = new ApiInteraction("http://localhost:5000");
api.getData((param) => {
  dataTable.initData(param);
});
/*
let buttonClear = document.querySelector('#delete-data');
buttonClear.addEventListener('click', ()=>{
  dataTable.clear();
});*/
