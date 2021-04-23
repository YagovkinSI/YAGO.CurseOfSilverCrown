import LoginPartial from './LoginPartial.jsx';

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
              <a className="nav-link active" onClick={() => props.changeActivePage('main')} href="">Главная</a>
            </li>
            <li className="nav-item">
              <a className="nav-link" onClick={() => props.changeActivePage('myOrganization')} href="">Моя провинция</a>
            </li>
            <li className="nav-item">
              <a className="nav-link" onClick={() => props.changeActivePage('provinces')} href="">Провинции</a>
            </li>
          </ul>
          <LoginPartial currentUser={props.currentUser} changeActivePage={props.changeActivePage} onUserLogout={props.onLogout}/>
        </div>
      </div>
    </nav>
  );
}
