function createAlert() {
    alert("Hey this is alert!");
}

function createPrompt(question) {
    return prompt(question);
}

function setElementTextById(id, text) {
    document.getElementById(id).innerText = text;
}

function focusOnElement(element) {
    element.focus();
}