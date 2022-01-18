'use strict'

import RadioButton from "./clases/RadioButton.js";

let addButton = document.querySelector("#addButton");
let container = document.querySelector(".radio-button-container");
let radioButtonID = 1;
let radiobuttons = [];

addButton.addEventListener("click",()=>{
    let options = {
        radioButtonID,
        container
    };

    let newButtonDelete = new RadioButton(options);
    newButtonDelete.createControl();

    radiobuttons.push(newButtonDelete);
    radioButtonID++;
});

let checkButton = document.querySelector("#checkButton");
checkButton.addEventListener("click",()=>{
    radiobuttons.forEach(el=> console.log(el.getValue()));
});

/*
function getRadioButtonValue(){
    let filter = document.querySelector(".filter-radiobutton:checked");
    return filter.value;
}

console.log(getRadioButtonValue());
*/