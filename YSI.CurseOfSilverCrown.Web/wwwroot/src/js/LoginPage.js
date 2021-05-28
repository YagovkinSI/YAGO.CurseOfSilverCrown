import { useState } from 'react';

export default function LoginPage(props) {
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');
  const [rememberMe, setRememberMe] = useState(false);

  function submitLoginInfo(e) {
    e.preventDefault();
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
    <div className="center full-screen stone-background">
      <section>
        <form onSubmit={(e) => submitLoginInfo(e)} >
          <h4>Используйте локальную учетную запись для входа в систему.</h4>
          <div className="form-group">
            <label>Логин</label>
            <input className="form-control form-frame"
              type="text"
              onChange={(e) => setLogin(e.target.value)}/>
          </div>
          <div className="form-group">
            <label>Пароль</label>
            <input className="form-control"
              type="password"
              onChange={(e) => setPassword(e.target.value)} />
          </div>
          <div className="form-group">
            <button type="submit" className="btn btn-primary">Вход</button>
          </div>
        </form>
      </section>
    </div>
  );
}
