export default class DropDownField {
    fieldList;
    fields;
    selectedFieldItem;

    constructor(options){
        if (options == null)
            throw new Error("Options for DropDownField is null.");

        if (options.dropDownFieldID == null)
            throw new Error("id is not be null");

        if (options.container == null)
            throw new Error("container for placement is null");
        
        if (options.captionsAndValues == null || options.captionsAndValues == undefined)
            throw new Error("field and values not set");

        this.dropDownFieldID = options.dropDownFieldID;
        this.container = options.container;
        this.captionsAndValues = options.captionsAndValues;
        this.fields = [];
    }

    createControl(){
        let img = document.createElement("img");
        img.alt = "show list";
        img.src = "img/arrow-down.svg";

        let img_container = document.createElement("div");
        img_container.classList.add("drop-down-field-button");

        this.textField = document.createElement("input");
        this.textField.type = "text";
        this.textField.classList.add("drop-down-field-text");
        this.textField.name = "drop-down-field-text";
        this.textField.placeholder = "Выберите поле"
        this.textField.readOnly = true;

        let textAndFieldContainer = document.createElement("div");
        textAndFieldContainer.classList.add("drop-down-field-text-and-button");
        
        this.fieldList = document.createElement("ul")
        this.fieldList.classList.add("drop-down-field-list")
         
        this.captionsAndValues.forEach(el => {
            this.fields.push(this.#createFieldItem(el.field, el.value));    
        });
         
        this.fields.forEach(el=> this.fieldList.appendChild(el));

        let dropDownfieldContainer = document.createElement("div");
        dropDownfieldContainer.classList.add("drop-down-field-container");

        img_container.appendChild(img);

        textAndFieldContainer.appendChild(this.textField);
        textAndFieldContainer.appendChild(img_container);

        dropDownfieldContainer.appendChild(textAndFieldContainer);
        dropDownfieldContainer.appendChild(this.fieldList);
        dropDownfieldContainer.id = "dropDownFieldContainer_"+this.dropDownFieldID;

        textAndFieldContainer.addEventListener("click",()=>{
            this.#toggleShowFieldList();
        });
        
        this.container.appendChild(dropDownfieldContainer);
    }

    #toggleShowFieldList(){
        this.fieldList.classList.toggle("show");
    }

    #createFieldItem(text, value){
        let item = document.createElement("li");
        item.classList.add("drop-down-field-item");
        item.tabIndex = "0";
        item.innerText = text;
        item.dataset.value = value;
        item.addEventListener("click",()=>{
            this.selectedFieldItem = {text, value};
            this.textField.value = text;
            this.#toggleShowFieldList();
        });

        return item;
    }

    getSelectedItem(){
        return this.selectedFieldItem;
    }

    getSelectedItemText(){
        if (this.selectedFieldItem != undefined)
            return this.selectedFieldItem.text;
        else
            return undefined;
    }

    getSelectedItemValue(){
        if (this.selectedFieldItem != undefined)
            return this.selectedFieldItem.value;
        else 
            return undefined;
    }
}