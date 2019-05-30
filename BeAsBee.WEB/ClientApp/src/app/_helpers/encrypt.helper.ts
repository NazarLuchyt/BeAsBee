
export function encryptMessage(str: string, amount: number) {
    let output = '';
    for (let i = 0; i < str.length; i++) {
        let c = str[i];
        const code = str.charCodeAt(i);
        c = String.fromCharCode(code + amount);
        output += c;
    }
    return output;
}
