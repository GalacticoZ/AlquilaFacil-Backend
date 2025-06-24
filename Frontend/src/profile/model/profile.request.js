export class ProfileRequest {
  constructor({name, fatherName, motherName, phone, documentNumber, dateOfBirth, bankAccountNumber, interbankAccountNumber}) {
    this.name = name;
    this.fatherName = fatherName;
    this.motherName = motherName;
    this.phone = phone;
    this.documentNumber = documentNumber;
    this.dateOfBirth = dateOfBirth;
    this.bankAccountNumber = bankAccountNumber;
    this.interbankAccountNumber = interbankAccountNumber;
  }
}