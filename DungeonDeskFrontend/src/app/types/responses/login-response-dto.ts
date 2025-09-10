export default class LoginResponseDTO {
  public constructor(readonly accessToken: string, readonly refreshToken: string) { }
}
