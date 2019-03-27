window.AddOnKeyDownEvent = () => {
  document.onkeydown = function (evt) {
    evt = evt || window.event;
    DotNet.invokeMethodAsync('BlazorSnake', 'JsKeyDown', evt.keyCode);

    //Prevent all but F5 and F12
    if (evt.keyCode !== 116 && evt.keyCode !== 123)
      evt.preventDefault();
  };
};
window.AddOnKeyUpEvent = () => {
  document.onkeyup = function (evt) {
    evt = evt || window.event;
    DotNet.invokeMethodAsync('BlazorSnake', 'JsKeyUp', evt.keyCode);
    //Prevent all but F5 and F12
    if (evt.keyCode !== 116 && evt.keyCode !== 123)
      evt.preventDefault();
  };
};
window.MySetFocus = (ctrl) => {
    document.getElementById(ctrl).focus();
    return true;
};
