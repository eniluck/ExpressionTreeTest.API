export default class DataTable{
    constructor(data, headers) {
        this.table = document.querySelector('#data-table');
        this.initHeader(headers);
        this.initData(data);
        this.$this = this;
    }

    clear() {
        let new_tbody = document.createElement('tbody');
        let old_tbody = this.table.querySelector('tbody');
        old_tbody.parentNode.replaceChild(new_tbody,old_tbody);
    }

    initHeader(headers) {
        this.headers = headers;

        let old_thead = this.table.querySelector('thead');

        let new_thead = document.createElement('thead');
        this.headers.forEach(el => {
            let new_th = document.createElement('th');
            new_th.innerText = el.caption;
            new_thead.appendChild(new_th);
        });

        old_thead.parentNode.replaceChild(new_thead,old_thead);
    }

    initData(data) {
        console.log(data);
        this.data = data;
        let new_tbody = document.createElement('tbody');
        
        
        this.data.forEach( data_row => {
            let new_tr = document.createElement('tr');

            this.headers.forEach(header => {
                let new_td = document.createElement('td');
                new_td.innerText = data_row[header.key];
                new_tr.appendChild(new_td);
                
            });

            new_tbody.appendChild(new_tr);
        });
        
        let old_tbody = this.table.querySelector('tbody');
        old_tbody.parentNode.replaceChild(new_tbody,old_tbody);
    }
}