'use strict'

import DataTable from './DataTable/dataTable.js';
import ApiInteraction from './API/ApiInteraction.js';
import Filter from './Filter.js';

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

let fields = [
  {
    name: "Id",
    type: "int"
  },
  {
    name: "Name",
    type: "string"
  },
  {
    name: "ReleaseYear",
    type: "int?"
  },
  {
    name: "ScreenDiagonal",
    type: "decimal?"
  },
  {
    name: "SimCardCount",
    type: "int?"
  },
  {
    name: "SimCardFormatName",
    type: "string"
  },
  {
    name: "Color",
    type: "string"
  },
]

let filters = [
  {
    name: "notset",
    value: "",
  },
  {
    name: "contains",
    value: "Содержит",
  },
  {
    name: "!contains",
    value: "Не содержит",
  },
  {
    name: "equals",
    value: "Равно",
  },
  {
    name: "!equals",
    value: "Не равно",
  },
  {
    name: "blank",
    value: "Пустая строка",
  },
  {
    name: "!blank",
    value: "Не пустая строка",
  },
  {
    name: "null",
    value: "null",
  },
  {
    name: "!null",
    value: "Не null",
  },
  {
    name: "starts",
    value: "Начинается",
  },
  {
    name: "!starts",
    value: "Не начинается",
  },
  {
    name: "ends",
    value: "Заканчивается",
  },
  {
    name: "!ends",
    value: "Не заканчивается",
  }
]

let phones = [

];

let dataTable = new DataTable(phones, header);
let api = new ApiInteraction("http://localhost:5000");

/*инициализация кнопок*/
let buttonClear = document.querySelector('.search__button-clear');
buttonClear.addEventListener('click', ()=>{
  dataTable.clear();
});

let buttonFind = document.querySelector('.search__button-find');
buttonFind.addEventListener('click', ()=>{
  api.getData((param) => {
    dataTable.initData(param);
  })
});

let filter = new Filter(fields,filters);
// добавление нового фильтра
filter.AddRow("Id");

// добавить добавление поля фильтра по нажатию кнопки.