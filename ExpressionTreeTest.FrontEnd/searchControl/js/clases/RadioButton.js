export default class RadioButton {
    constructor(options) {
        if (options == null)
            throw new Error("Options for RadioButton is null.")        

        if (options.radioButtonID == null)
            throw new Error("id is not be null")

        if (options.container == null)
            throw new Error("container for placement is null")

        this.radioButtonID = options.radioButtonID;
        this.container = options.container;
    }

    createControl(){
        let radioButtonAnd = this.#createRadioButtonInput("radioButtonInput_and","true","AND");
        let radioButtonOR = this.#createRadioButtonInput("radioButtonInput_or", null,"OR");
        let radioLabelAnd = this.#createRadioButtonLabel("radioButtonInput_and","И");
        let radioLabelOR = this.#createRadioButtonLabel("radioButtonInput_or","ИЛИ");
        
        this.radioButtonContainer = document.createElement("div");  
        this.radioButtonContainer.classList.add("radio-button");

        this.radioButtonContainer.appendChild(radioButtonAnd);
        this.radioButtonContainer.appendChild(radioLabelAnd);
        this.radioButtonContainer.appendChild(radioButtonOR);
        this.radioButtonContainer.appendChild(radioLabelOR);

        this.container.appendChild(this.radioButtonContainer);
    }

    #createRadioButtonInput(id_prefix, checked, value){
        let radioButtonNamePrefix = "radiobutton"

        let radioButton = document.createElement("input");  
        radioButton.classList.add("radio-button-input");
        radioButton.type = "radio";
        radioButton.name = radioButtonNamePrefix+this.radioButtonID;
        radioButton.id = id_prefix+this.radioButtonID;
        radioButton.checked = checked;
        radioButton.value = value;

        return radioButton;
    }

    #createRadioButtonLabel(name, text){
        let radioLabel = document.createElement("label");
        radioLabel.classList.add("radio-button-label");
        radioLabel.htmlFor = name+this.radioButtonID;
        radioLabel.innerText = text;
        return radioLabel;
    }

    getValue(){
        let checkedRadioButton = this.radioButtonContainer.querySelector("input:checked");
        return checkedRadioButton.value;
    }
}