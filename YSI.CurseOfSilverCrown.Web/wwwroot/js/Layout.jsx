import HomeIndex from './HomeIndex.jsx';
import LoginPage from './LoginPage.jsx';
import LoginPartial from './LoginPartial.jsx';

export default function Layout () {
  const [currentUser, setCurrentUser] = React.useState({ isSignedIn: false, userName: '' });
  const [page, setPage] = React.useState('main');

  React.useEffect(() => {
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

  const onLogin = (login) => {
    setCurrentUser({ isSignedIn: true, userName: login });
  }

  const changeMainPage = (pageName) => {
    setPage(pageName);
  }

  const onLogout = () => {
    setCurrentUser({ isSignedIn: false, userName: '' });
  }

  let mainPage = null;
  
  switch(page) {
    case 'main':
      mainPage = (<HomeIndex />);
      break;
    case 'login':
      mainPage = (<LoginPage onUserLogged={onLogin} />);
      break;
    default:
      mainPage = (<h1>eshsrth</h1>);
      break;
  };

  return (
    <div>
      <header>
        <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
          <div className="container">
            <a className="navbar-brand" > {/*asp-area="" asp-controller="Home" asp-action="Index"*/}
              Проклятие Серебрянной Короны
            </a>
            <button className="navbar-toggler" type="button" data-toggle="collapse" data-target=".navbar-collapse" aria-controls="navbarSupportedContent"
              aria-expanded="false" aria-label="Toggle navigation">
              <span className="navbar-toggler-icon"></span>
            </button>
            <div className="navbar-collapse collapse d-sm-inline-flex justify-content-between">
              <ul className="navbar-nav flex-grow-1">
                <li className="nav-item">
                  <a className="nav-link text-dark" onClick={() => setPage('main')}>Главная</a>
                </li>
                <li className="nav-item">
                  <a className="nav-link text-dark" onClick={() => setPage('myOrganization')}>Моя провинция</a>
                </li>
                <li className="nav-item">
                  <a className="nav-link text-dark" onClick={() => setPage('provinces')}>Провинции</a>
                </li>
              </ul>
              <LoginPartial currentUser={currentUser} changeMainPage={changeMainPage} onUserLogout={onLogout}/>
            </div>
          </div>
        </nav>
      </header>;
      <div className="container">
        <main role="main" className="pb-3">
          {mainPage}
        </main>
      </div>
      <footer className="border-top footer text-muted">
        <div className="container">
          &copy; 2021 - Проклятие Серебрянной Короны {/*- <a asp-area="" asp-controller="Home" asp-action="Privacy">Политика конфиденциальности</a>*/}
        </div>
      </footer>
    </div>
  );
}
