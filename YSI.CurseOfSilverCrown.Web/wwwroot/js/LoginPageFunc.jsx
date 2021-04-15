import { useState } from "react";

export default function LoginPage(props) {
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');
  const [rememberMe, setRememberMe] = useState(false);

  function submitLoginInfo() {
    if(!login || !password) return;
    
    const data = new FormData();
    data.append("userName", login);
    data.append("password", password);
    data.append("rememberMe", rememberMe);
    var xhr = new XMLHttpRequest();

    xhr.open("post", "/api/Auth", true);
    xhr.onload = () => {
      if (xhr.status === 200) {
        props.onUserLogged(login);
      }
      else {
        alert("Неверный логин или пароль.");
      }
    };
    xhr.send(data);
  }

  return (
    <div>
      <h1>Вход</h1>
      <div className="row">
        <div className="col-md-4">
          <section>
            <form onSubmit={submitLoginInfo} >
              <h4>Используйте локальную учетную запись для входа в систему.</h4>
              <hr />
              <div asp-validation-summary="All" className="text-danger"></div>
              <div className="form-group">
                <label>Логин</label>
                <input className="form-control"
                  type="text"
                  onChange={(e) => setLogin(e.target.value)}/>
              </div>
              <div className="form-group">
                <label>Пароль</label>
                <input className="form-control"
                  type="password"
                  onChange={(e) => setPassword(e.target.value)} />
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
    </div>
  );
}
