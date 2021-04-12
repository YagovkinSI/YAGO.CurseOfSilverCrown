export default class LoginPage extends React.Component {
  constructor(props) {
      super(props);

      this.onSubmit = this.onSubmit.bind(this);
      this.checkUser = this.checkUser.bind(this);
  }

  onSubmit(e) {
      e.preventDefault();
      var userName = this.refs.nameInput.value;
      var userPassword = this.refs.passwordInput.value;
      if (!userName || !userPassword) {
          return;
      }
      this.checkUser(userName, userPassword);            
  }

  checkUser(name, password) {
      const data = new FormData();
      data.append("userName", name);
      data.append("password", password);
      data.append("rememberMe", false);
      var xhr = new XMLHttpRequest();

      xhr.open("post", "/api/Auth", true);
      xhr.onload = function () {
          if (xhr.status === 200) {
              this.props.onUserLogged(name);
          }
          else {
              alert("Неверный логин или пароль.");
          }
      }.bind(this);
      xhr.send(data);
  }
  
  render() {
      return (
        <div>
            <h1>Вход</h1>
            <div className="row">
                <div className="col-md-4">
                    <section>
                        <form onSubmit={this.onSubmit} >
                            <h4>Используйте локальную учетную запись для входа в систему.</h4>
                            <hr />
                            <div asp-validation-summary="All" className="text-danger"></div>
                            <div className="form-group">
                                <label>Логин</label>
                                <input className="form-control"
                                    type="text"
                                    ref="nameInput" />
                            </div>
                            <div className="form-group">
                                <label>Пароль</label>
                                <input className="form-control"
                                    type="password"
                                    ref="passwordInput" />
                            </div>
                            {/*<div className="form-group">*/}
                            {/*    <div className="checkbox">*/}
                            {/*        <label asp-for="Input.RememberMe">*/}
                            {/*            <input asp-for="Input.RememberMe" />*/}
                            {/*        @Html.DisplayNameFor(m => m.Input.RememberMe)*/}
                            {/*        </label>*/}
                            {/*    </div>*/}
                            {/*</div>*/}
                            <div className="form-group">
                                <button type="submit" className="btn btn-primary">Вход</button>
                            </div>
                            {/*<div className="form-group">*/}
                            {/*    <p>*/}
                            {/*        <a id="forgot-password" asp-page="./ForgotPassword">Забыли Ваш пароль?</a>*/}
                            {/*    </p>*/}
                            {/*    <p>*/}
                            {/*        <a>*/}{/*asp-page="./Register" asp-route-returnUrl="@Model.ReturnUrl"*/}
                            {/*            Зарегистрируйтесь как новый пользователь*/}
                            {/*        </a>*/}
                            {/*    </p>*/}
                            {/*    <p>*/}
                            {/*        <a id="resend-confirmation" asp-page="./ResendEmailConfirmation">Повторно отправить подтверждение по электронной почте</a>*/}
                            {/*    </p>*/}
                            {/*</div>*/}
                        </form>
                    </section>
                </div>
            </div>
        </div>);
  }
}
