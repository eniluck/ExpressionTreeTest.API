'use strict'

import ButtonAddWithMenu from "./clases/ButtonAddWithMenu.js";

let addButton = document.querySelector("#addButton");
let container = document.querySelector(".button-add-container");

let i=0;

addButton.addEventListener("click",()=>{
    let onClickFunctions = {
        group: function(id) {
          console.log("group click "+ id);
        },
        condition: function(id) {
            console.log("condition click "+ id);
        }
    };

    let options = {
        addButtonID: i,
        container,
        onClickFunctions : {
            group: function(id) {
              console.log("group click "+ id);
            },
            condition: function(id) {
                console.log("condition click "+ id);
            }
        }
    };

    let buttonAddWithMenu = new ButtonAddWithMenu(options);
    buttonAddWithMenu.createControl();

    i++;
});
