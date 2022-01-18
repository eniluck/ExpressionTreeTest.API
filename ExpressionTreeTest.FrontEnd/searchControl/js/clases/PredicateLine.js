const fieldAndValues = [
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

const filtersAndValues = [
    {
        field: "пусто",
        value: "blank"
    },
    {
        field: "не пусто",
        value: "!blank"
    },
    {
        field: "содержит",
        value: "contains"
    },
    {
        field: "не содержит",
        value: "!contains"
    },
    {
        field: "заканчивается",
        value: "ends"
    },
    {
        field: "не заканчивается",
        value: "!ends"
    },
    {
        field: "равно",
        value: "equals"
    },
    {
        field: "не равно",
        value: "!equals"
    },
    {
        field: "больше",
        value: ">"
    },
    {
        field: "больше или равно",
        value: ">="
    },
    {
        field: "меньше",
        value: "<"
    },
    {
        field: "меньше или равно",
        value: "<="
    },
    {
        field: "задано",
        value: "!null"
    },
    {
        field: "не задано",
        value: "null"
    },
    {
        field: "начинается",
        value: "starts"
    },
    {
        field: "не начинается",
        value: "!starts"
    } 
];
const fieldID = 1;
const filterID =1;
const textFieldID = 1;
const buttonDeleteID = 1;

import DropDownField from "./DropDownField.js";
import TextField from "./TextField.js";
import ButtonDelete from "./ButtonDelete.js";


export default class PredicateLine {
    constructor(container){
        if (container == null)
            throw new Error("container for placement is null");

        this.container = container;
    }

    createControl(){
        this.predicateLineDiv = document.createElement("div");
        this.predicateLineDiv.classList.add("predicate-line");

        let predicatesDiv = document.createElement("div");
        predicatesDiv.classList.add("predicates");

        let predicateField = document.createElement("div");
        predicateField.classList.add("predicate-field");

        let predicateFilter = document.createElement("div");
        predicateFilter.classList.add("predicate-filter");

        this.predicateValueDiv = document.createElement("div");
        this.predicateValueDiv.classList.add("predicate-value");

        this.#createFieldDropDown(predicateField);
        this.#createFilterDropDown(predicateFilter);
        this.#createTextField();

        predicatesDiv.appendChild(predicateField);
        predicatesDiv.appendChild(predicateFilter);
        predicatesDiv.appendChild(this.predicateValueDiv);
        this.predicateLineDiv.appendChild(predicatesDiv);
        this.container.appendChild(this.predicateLineDiv);

        this.#createButtonDelete();
    }

    #createFieldDropDown(container){
        let newFieldOptions = {
            dropDownFieldID: fieldID,
            container: container,
            captionsAndValues: fieldAndValues
        }
        this.dropDownField  = new DropDownField(newFieldOptions);
        this.dropDownField.createControl();
    }

    #createFilterDropDown(container){
        let newFilterOptions = {
            dropDownFieldID: filterID,
            container: container,
            captionsAndValues: filtersAndValues
        }
        this.dropDownFilter = new DropDownField(newFilterOptions);
        this.dropDownFilter.createControl();
    }

    #createTextField(){
        let textFieldOptions = {
            textFieldID,
            container: this.predicateValueDiv
        }

        this.newTextField = new TextField(textFieldOptions);
        this.newTextField.createControl();
    }

    #createButtonDelete(){
        let onClickFunction = (id)=> {
            //console.log('id = ' + id);
            this.predicateLineDiv.remove();
        };
    
        let options = {
            buttonDeleteID,
            container: this.predicateLineDiv,
            onClickFunction: onClickFunction
        };
    
        let newButtonDelete = new ButtonDelete(options);
    
        newButtonDelete.createControl();
    }

    getValue(){
        let dropDownFieldValue = this.dropDownField.getSelectedItemValue();
        let dropDownFilterValue = this.dropDownFilter.getSelectedItemValue();
        let textValue = this.newTextField.getValue();
        return {
            field: dropDownFieldValue,
            filter: dropDownFilterValue,
            text: textValue
        }
    }
}