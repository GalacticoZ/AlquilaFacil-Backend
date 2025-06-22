export class ProfileResponse {
  constructor({id, fullName, phone, documentNumber, dateOfBirth, bankAccountNumber, interbankAccountNumber}) {
    this.id = id;
    this.fullName = fullName;
    this.name = fullName.split(' ')[0];
    this.fatherName = fullName.split(' ')[1];
    this.motherName = fullName.split(' ')[2];
    this.phone = phone;
    this.documentNumber = documentNumber;
    this.dateOfBirth = dateOfBirth;
    this.bankAccountNumber = bankAccountNumber;
    this.interbankAccountNumber = interbankAccountNumber;
  }
}