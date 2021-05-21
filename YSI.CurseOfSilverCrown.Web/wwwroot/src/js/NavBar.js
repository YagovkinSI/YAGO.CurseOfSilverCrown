import { NavLink } from 'react-router-dom';
import AuthNav from "./AuthNav";
import NotAuthNav from "./NotAuthNav";

export default function NavBar(props) {
  return (
    <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
      <div className="container">
        <NavLink className="navbar-brand" exact to="/">Проклятие Серебряной Короны</NavLink>
        {/*asp-area="" asp-controller="Home" asp-action="Index"*/}
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
