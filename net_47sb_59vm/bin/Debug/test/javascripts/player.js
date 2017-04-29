var ga = document.createElement('script');
    ga.type = 'text/javascript';
    ga.async = false;
    ga.src = 'http://www.youtube.com/player_api';
    var s = document.getElementsByTagName('script')[0];
    s.parentNode.insertBefore(ga, s);

    var done = false;
    var player;

    function onYouTubePlayerAPIReady() {
        player = new YT.Player('video', {
            height: '254',
            width: '450',
            videoId: '_B789lus-JE',
        playerVars: {
            controls: 0,
            disablekb: 0,
            fs:0,
            origin: '127.0.0.1',
            rel:0
        },
          events: {
            'onReady': onPlayerReady,
            'onStateChange': onPlayerStateChange,           
          }
        });
    }
    function onPlayerReady(evt) {
        evt.target.playVideo();
    }
    function onPlayerStateChange(evt) {
        /*if (evt.data == YT.PlayerState.PLAYING && !done) {
            //setTimeout(stopVideo, 6000);
            done = true;
        }*/
    }


    function stopVideo() {
        //console.log('stopVideo');
        player.stopVideo();
    }