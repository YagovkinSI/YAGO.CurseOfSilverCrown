import AuthNav from "./AuthNav.jsx";
import NotAuthNav from "./NotAuthNav.jsx";

const NavLink = window.ReactRouterDOM.NavLink;

export default function NavBar(props) {
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
              <NavLink className="nav-link" exact to="/">Главная</NavLink>
            </li>
            <li className="nav-item">
              <NavLink className="nav-link" to="/my-province">Моя провинция</NavLink>
            </li>
            <li className="nav-item">
              <NavLink className="nav-link" to="/provinces">Провинции</NavLink>
            </li>
          </ul>
          {props.currentUser.isSignedIn ? 
            <AuthNav userName={props.currentUser.userName} onLogout={props.onLogout}/> 
            : <NotAuthNav />}
        </div>
      </div>
    </nav>
  );
}
