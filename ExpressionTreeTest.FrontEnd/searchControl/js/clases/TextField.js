export default class TextField {
    constructor(options){
        if (options == null)
            throw new Error("Options for TextField is null.");

        if (options.textFieldID == null)
            throw new Error("id is not be null");

        if (options.container == null)
            throw new Error("container for placement is null");

        this.textFieldID = options.textFieldID;
        this.container = options.container;
    }

    createControl(){
        this.textField = document.createElement("input");
        this.textField.type = "text";
        this.textField.classList.add("text-field");
        this.textField.name = "text-field";
        this.textField.placeholder = "Введите значение"

        let textFieldContainer = document.createElement("div");
        textFieldContainer.classList.add("text-field-container");
        textFieldContainer.id= "text-field-container_"+ this.textFieldID;

        textFieldContainer.appendChild(this.textField);

        this.container.appendChild(textFieldContainer);
    }

    getValue(){
        return this.textField.value;
    }
}