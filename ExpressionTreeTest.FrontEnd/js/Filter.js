export default class Filter{
    constructor(fileds,filters) {
        this.available_fileds = fileds;
        this.available_filters = filters;
    }

    GetNewFilterData(){
        let filterNameSelect = document.querySelector(".search__row-name");
        let fieldName = filterNameSelect.value;
        return fieldName;
    }

    InitializeFilterNames(){
        let filterNameSelect = document.querySelector(".search__row-name");

        let i, L = filterNameSelect.options.length - 1;
        for(i = L; i >= 0; i--) {
            filterNameSelect.remove(i);
        }

        this.available_fileds.forEach(el => {
            var selectOption = document.createElement('option');
            selectOption.value = el.name;
            selectOption.innerHTML = el.name;
            filterNameSelect.appendChild(selectOption);
        });
    }

    AddRow(fieldName){
        let field = this.available_fileds.find(x => x.name === fieldName);
        //if not found ?

        let new_tr = document.createElement('tr');
        new_tr.classList.add("search__row");
        
        let td_with_fieldName = document.createElement('td');
        
        let labelFieldName = document.createElement('label');
        labelFieldName.htmlFor = 'field-name'+fieldName;
        labelFieldName.classList.add("search__row-label");
        labelFieldName.innerText = field.name; // а может быть и Caption 

        td_with_fieldName.appendChild(labelFieldName)


        let td_with_select = document.createElement('td');
        let select = document.createElement('select');
        select.classList.add("search__row-select");

        //взять список всех доступных фильтров
        this.available_filters.forEach(el => {
            var selectOption = document.createElement('option');
            selectOption.value = el.name;
            selectOption.innerHTML = el.value;
            select.appendChild(selectOption);
        });
        

        td_with_select.appendChild(select);


        let td_with_text = document.createElement('td');

        let input = document.createElement('input');
        input.id = "field-name"+fieldName;
        input.type = "text";
        input.name = "field-name"+fieldName;
        input.classList.add("search__row-text");

        td_with_text.appendChild(input);

        new_tr.appendChild(td_with_fieldName);
        new_tr.appendChild(td_with_select);
        new_tr.appendChild(td_with_text);

        let search = document.querySelector('.search__table tbody');
        search.appendChild(new_tr);
    }

    /*Получить значения из всех полей фильтра*/
    GetFilterValue(){
        let search = document.querySelector('.search__table tbody');
        //пройти по всем tr для search и по каждому взять имя поля, значение фильтра и значение поля
        getFieldName
        let result = [
            {
                fieldName: "ID",
                filterValue: "",
                fieldValue: ""
            }
        ]
    }
}