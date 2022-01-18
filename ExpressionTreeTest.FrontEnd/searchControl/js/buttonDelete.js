'use strict'

import ButtonDelete from "./clases/ButtonDelete.js";

let addButton = document.querySelector("#addButton");
let container = document.querySelector(".button-delete-container");
let buttonDeleteID = 1;

addButton.addEventListener("click",()=>{
    let onClickFunction = (id)=> {
        console.log('id = ' + id);
    };

    let options = {
        buttonDeleteID,
        container,
        onClickFunction
    };

    let newButtonDelete = new ButtonDelete(options);

    newButtonDelete.createControl();
    buttonDeleteID++;
});