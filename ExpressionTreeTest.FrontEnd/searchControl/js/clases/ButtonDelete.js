export default class ButtonDelete {
    constructor(options){
        if (options == null)
            throw new Error("Options for ButtonDelete is null.")

        if (options.buttonDeleteID == null)
            throw new Error("id is not be null")

        if (options.container == null)
            throw new Error("container for placement is null")

        if (typeof options.onClickFunction != 'function') {
            throw new Error("param must be function")
        }

        this.buttonDeleteID = options.buttonDeleteID;
        this.container = options.container;
        this.onClickFunction = options.onClickFunction;
    }

    createControl(){
        let img = document.createElement("img");
        img.alt = "delete";
        img.src = "img/minus.svg";

        let div = document.createElement("div");
        div.classList.add("button-delete");

        div.appendChild(img);

        div.addEventListener("click",()=>{
            this.onClickFunction(this.buttonDeleteID);
        });

        this.container.appendChild(div);
    }
}