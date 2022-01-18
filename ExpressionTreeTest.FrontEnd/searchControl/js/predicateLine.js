'use strict'

import PredicateLine from "./clases/PredicateLine.js";

let addButton = document.querySelector("#add");
let container = document.querySelector(".predicateLines-container");
let dropDownFieldID = 1;
let dropDownFields = [];
let fieldAndValues = [
    {
        field: "field1",
        value: "value1"
    },
    {
        field: "field2",
        value: "value2"
    },
    {
        field: "field3",
        value: "value3"
    },
    {
        field: "field4",
        value: "value4"
    },
    {
        field: "field4",
        value: "value"
    },
];

addButton.addEventListener("click",()=>{
    let newPredicateLine = new PredicateLine(container);
    dropDownFields.push(newPredicateLine);
    newPredicateLine.createControl();
    dropDownFieldID++;
});

let checkButton = document.querySelector("#checkButton");
checkButton.addEventListener("click",()=>{
    dropDownFields.forEach(el=> console.log(el.getValue()));
});
