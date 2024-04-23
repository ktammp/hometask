window.addEventListener("load", () => {
    clock();
    function clock() {
        const today = new Date();


        const hours = today.getHours();
        const minutes = today.getMinutes();
        const seconds = today.getSeconds();

        const hour = hours < 10 ? "0" + hours : hours;
        const minute = minutes < 10 ? "0" + minutes : minutes;
        const second = seconds < 10 ? "0" + seconds : seconds;

        const hourTime = hour;


        let month = today.getMonth() + 1;
        month = month < 10 ? "0" + month : month;
        const year = today.getFullYear();
        let day = today.getDate();
        day = day < 10 ? "0" + day : day;



        const date = day + "." + month + "." + year;
        const time = hourTime + ":" + minute + ":" + second;

        const dateTime = date + " - " + time;

        document.getElementById("date-time").innerHTML = dateTime;
        setTimeout(clock, 1000);
    }
});