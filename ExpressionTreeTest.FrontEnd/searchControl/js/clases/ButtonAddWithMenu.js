export default class ButtonAddWithMenu {
    container;

    groupFunction;
    conditionFunction;

    buttonWithMenu;
    buttonAdd;
    ulMenu;
    liGroup;
    liCondition;

    constructor(options){
        if (options == null)
            throw new Error("Options for ButtonAddWithMenu is null.");

        if (options.addButtonID == null)
            throw new Error("Options for ButtonAddWithMenu is null.");        

        if (options.container == null)
            throw new Error("container for placement is null");
        
        if (typeof options.onClickFunctions.group != 'function') {
            throw new Error("param must be function")
        }

        if (typeof options.onClickFunctions.condition != 'function') {
            throw new Error("param must be function")
        }            
        
        this.addButtonID = options.addButtonID;
        this.groupFunction = options.onClickFunctions.group;
        this.conditionFunction = options.onClickFunctions.condition;
        this.container = options.container;
    }

    createControl(){
        this.#createButtonWithMenu();
        this.#addEventListeners();
    }

    #createButtonWithMenu(){
        this.#createMenu();
        this.#createButton();

        this.buttonWithMenu = document.createElement("div");
        this.buttonWithMenu.classList.add("button-add-with-menu");
        this.buttonWithMenu.appendChild(this.buttonAdd);
        this.buttonWithMenu.appendChild(this.ulMenu);
        this.container.appendChild(this.buttonWithMenu);
    }

    #createMenu(){
        this.liGroup = document.createElement("li");
        this.liGroup.classList.add("menu-item");
        this.liGroup.innerText="Добавить группу";

        this.liCondition = document.createElement("li");
        this.liCondition.classList.add("menu-item");
        this.liCondition.innerText="Добавить выражение";

        this.ulMenu = document.createElement("ul");
        this.ulMenu.classList.add("menu")

        this.ulMenu.appendChild(this.liGroup);
        this.ulMenu.appendChild(this.liCondition);
    }

    #createButton(){
        let img = document.createElement("img");
        img.alt = "add";
        img.src = "img/plus.svg";

        this.buttonAdd = document.createElement("div");
        this.buttonAdd.classList.add("button-add");
        this.buttonAdd.appendChild(img);
    }

    #addEventListeners(){
        this.buttonAdd.addEventListener("click", ()=>{
            this.ulMenu.classList.toggle('show');
        });

        this.liGroup.addEventListener("click", ()=>{
            this.groupFunction(this.addButtonID);
            this.ulMenu.classList.toggle('show');
        });

        this.liCondition.addEventListener("click", ()=>{
            this.conditionFunction(this.addButtonID);
            this.ulMenu.classList.toggle('show');
        });
    }
}