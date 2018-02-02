(function ($) {
    function ModalPop() {
        var $this = this;

        function initilizeModel() {
            $("#modal-popUP").on('loaded.bs.modal',
                function (e) {

                }).on('hidden.bs.modal',
                function (e) {
                    $(this).removeData('bs.modal');
                });
        }

        $this.init = function () {
            initilizeModel();
        };
    }

    $(function () {
        var self = new ModalPop();
        self.init();
    });
}(jQuery)); 