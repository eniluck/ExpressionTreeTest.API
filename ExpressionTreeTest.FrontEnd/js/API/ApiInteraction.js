export default class ApiInteraction {
    constructor(url){
        this.url = url
    }

    async getData(func) {
        let query = {
            filterParams: [
                {
                  fieldName: "ScreenDiagonal",
                  filterType: ">",
                  fieldValue: "6"
                }
              ],
              filterRule: "0",
              orderParams: [
                {
                  fieldName: "Id",
                  order: "desc"
                }
              ],
              pageNumber: 1,
              pageSize: 10
          };

        let params  = {
            method: 'POST',
            headers: {
                'Content-Type' : 'application/json;charset=utf-8'
            },
            body: JSON.stringify(query)
        }

        let response = await fetch(this.url+"/api/Phones", params);

        if (response.ok) {
            let phones_response = await response.json();
             
            func(phones_response.phones);
        } else {
            alert("Ошибка http:" + response.status);
        }
    }
}