import LoginPartial from './LoginPartial.jsx';
const Link = window.ReactRouterDOM.Link;

export default function NavBar(props) {
  const userAuthorised = (
    <ul className="navbar-nav">
      <li className="nav-item">
        <a className="nav-link text-dark" >
          Здравствуйте, {props.currentUser.userName}!
        </a>
      </li>
      <li className="nav-item">
        <form className="form-inline">
          <button className="nav-link btn btn-link text-dark" onClick={props.onLogout}>Выйти</button>
        </form>
      </li>
    </ul >);

    const userNotAuthorised = (
      <ul className="navbar-nav">
        <li className="nav-item">
          <Link className="nav-link" to="/login">Войти</Link>
        </li>
      </ul >);

  return (
    <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
      <div className="container-fluid">
        <a className="navbar-brand user-select-none" > {/*asp-area="" asp-controller="Home" asp-action="Index"*/}
          Проклятие Серебрянной Короны
        </a>
        <button className="navbar-toggler" type="button" data-toggle="collapse" data-target=".navbar-collapse" aria-controls="navbarSupportedContent"
          aria-expanded="false" aria-label="Toggle navigation">
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="navbar-collapse collapse d-sm-inline-flex justify-content-between">
          <ul className="navbar-nav flex-grow-1">
            <li className="nav-item">
              <Link className="nav-link" to="/">Главная</Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link" to="/my-province">Моя провинция</Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link" to="/provinces">Провинции</Link>
            </li>
          </ul>
          {props.currentUser.isSignedIn ? userAuthorised : userNotAuthorised}
        </div>
      </div>
    </nav>
  );
}
