const NavLink = window.ReactRouterDOM.NavLink;

export default function AuthNav(props) {
  return (
    <ul className="navbar-nav">
      <li className="nav-item">
        <NavLink className="nav-link text-dark" to="/manage-account">Здравствуйте, {props.userName}!</NavLink>
        {/* asp-area="Identity" asp-page="/Account/Manage/Index" title="Manage" */}
      </li>
      <li className="nav-item">
        <form className="form-inline">
          <button className="nav-link btn btn-link" onClick={props.onLogout}>Выйти</button>
        </form>
      </li>
    </ul>
  );
}
