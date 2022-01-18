'use strict'

import DropDownField from "./clases/DropDownField.js";

let addButton = document.querySelector("#add");
let container = document.querySelector(".drop-down-fields-container");
let dropDownFieldID = 1;
let dropDownFields = [];
let captionsAndValues = [
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
    let options = {
        dropDownFieldID,
        container,
        captionsAndValues
    }

    let newDropDownField = new DropDownField(options);
    dropDownFields.push(newDropDownField);
    newDropDownField.createControl();
    dropDownFieldID++;
});

let checkButton = document.querySelector("#checkButton");
checkButton.addEventListener("click",()=>{
    dropDownFields.forEach(el=> console.log(el.getSelectedItem()));
});
