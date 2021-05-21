import '../css/App.css';
import { useState, useEffect } from 'react';
import { HashRouter, Switch, Route, Redirect } from 'react-router-dom';
import NavBar from './NavBar';
import LoginPage from './LoginPage';
import Map from './Map';
import HomeIndex from './HomeIndex';

function App() {
  const [currentUser, setCurrentUser] = useState({ isSignedIn: false, userName: '' });

  useEffect(() => {
    var xhr = new XMLHttpRequest();
    xhr.open("get", "/api/Auth", true);
    xhr.onload = () => {
      if (xhr.status === 200) {
        if (xhr.responseText !== '') {
          setCurrentUser({ isSignedIn: true, userName: xhr.responseText });
        }
      }
    };
    xhr.send();
  }, []);

  return (
    <HashRouter>
      <div>
        <header>
          <NavBar currentUser={currentUser} onLogout={() => setCurrentUser({ isSignedIn: false, userName: '' })}/>
        </header>
        <div className="container">
          <main role="main" className="pb-3">
            <Switch>
              <Route path="/provinces">
                <h1>eshsrth</h1>
              </Route>
              {currentUser.isSignedIn ? null : <Redirect from="/my-province" to="/login"/>}
              <Route path="/my-province">
                <h1>eshsrth</h1>
              </Route>
              {currentUser.isSignedIn ? <Redirect from="/login" to="/"/> : null}
              <Route path="/login">
                <LoginPage onUserLogged={(login) => setCurrentUser({ isSignedIn: true, userName: login })}/>
              </Route>
              <Route path="/map">
                <Map />
              </Route>
              <Route path="/">
                <HomeIndex />
              </Route>
            </Switch>
          </main>
        </div>
        <footer className="border-top footer text-muted">
          <div className="container">
            &copy; 2021 - Проклятие Серебрянной Короны {/*- <a asp-area="" asp-controller="Home" asp-action="Privacy">Политика конфиденциальности</a>*/}
          </div>
        </footer>
      </div>
    </HashRouter>
  );
}

export default App;
