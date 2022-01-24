mergeInto(LibraryManager.library, {
  Web3Connect: function () {
    window.web3gl.connect();
  },

  ConnectAccount: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.connectAccount) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.connectAccount, buffer, bufferSize);
    return buffer;
  },

  SetConnectAccount: function (value) {
    window.web3gl.connectAccount = value;
  },

  SendContract: function (method, abi, contract, args, value) {
    window.web3gl.sendContract(
      Pointer_stringify(method),
      Pointer_stringify(abi),
      Pointer_stringify(contract),
      Pointer_stringify(args),
      Pointer_stringify(value)
    );
  },

  SendContractResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.sendContractResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.sendContractResponse, buffer, bufferSize);
    return buffer;
  },

  SetContractResponse: function (value) {
    window.web3gl.sendContractResponse = value;
  },

  SignMessage: function (message) {
    window.web3gl.signMessage(Pointer_stringify(message));
  },

  SignMessageResponse: function () {
    var bufferSize = lengthBytesUTF8(window.web3gl.signMessageResponse) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(window.web3gl.signMessageResponse, buffer, bufferSize);
    return buffer; 
  },

  SetSignMessageResponse: function (value) {
    window.web3gl.signMessageResponse = value;
  },

  GetNetwork: function () {
    return window.web3gl.network;
  }
});