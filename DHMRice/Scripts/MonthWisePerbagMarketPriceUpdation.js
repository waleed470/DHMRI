async function JsonCall(Method, Controller, Action) {
    $.ajax({
        type: Method.toUpperCase(),
        traditional: true,
        async: false,
        cache: false,
        url: '/' + Controller + '/' + Action,
        context: document.body,
        success: function (json) {
            return json;
        },
        error: function (xhr) {
            return xhr;
        }
    });
   
    
}
JsonCall('get', 'RawRice', 'MonthWisePerbagMarketPriceUpdation');