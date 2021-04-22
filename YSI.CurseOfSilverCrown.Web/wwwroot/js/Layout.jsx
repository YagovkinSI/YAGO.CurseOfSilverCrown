import HomeIndex from './HomeIndex.jsx';
import LoginPage from './LoginPage.jsx';
import NavBar from './NavBar.jsx';

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

  const changeActivePage = (pageName) => {
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
        <NavBar currentUser={currentUser} changeActivePage={changeActivePage} onUserLogout={onLogout}/>
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
