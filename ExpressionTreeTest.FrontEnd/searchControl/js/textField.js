'use strict'

import TextField from "./clases/TextField.js";

let addButton = document.querySelector("#add");
let container = document.querySelector(".text-fields-container");
let textFieldID = 1;
let textFields = [];

addButton.addEventListener("click",()=>{
    let options = {
        textFieldID,
        container
    };
    let newTextField = new TextField(options);
    newTextField.createControl();
    textFields.push(newTextField);
    
    textFieldID++;
});

let checkButton = document.querySelector("#checkButton");
checkButton.addEventListener("click",()=>{
    textFields.forEach(el=> console.log(el.getValue()));
});
