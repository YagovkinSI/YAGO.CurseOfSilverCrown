import HomeIndex from './HomeIndex.jsx';
import LoginPage from './LoginPage.jsx';
import LoginPartial from './LoginPartial.jsx';

export default class Layout extends React.Component {
  constructor(props) {
      super(props);
      this.state = {
          currentUser: {
              isSignedIn: false,
              userName: ""
          },
          page: "main"
      };
      this.changeMainPage = this.changeMainPage.bind(this); 
      this.onHomeClick = this.onHomeClick.bind(this);
      this.onMyOrganizationClick = this.onMyOrganizationClick.bind(this); 
      this.onProvincesClick = this.onProvincesClick.bind(this);
      this.onUserLogged = this.onUserLogged.bind(this); 
      this.onUserLogout = this.onUserLogout.bind(this); 
  }
  loadData() {
      var xhr = new XMLHttpRequest();
      xhr.open("get", "/api/Auth", true);
      xhr.onload = function () {
          if (xhr.status === 200) {
              if (xhr.responseText != "")
                  this.onUserLogged(xhr.responseText);
          }
      }.bind(this);
      xhr.send();
  }
  componentDidMount() {
      this.loadData();
  }
  changeMainPage(pageName) {
      this.setState({ page: pageName });
  }
  onHomeClick() {
      this.changeMainPage("main");
  }
  onMyOrganizationClick() {
      this.changeMainPage("myOrganization");
  }
  onProvincesClick() {
      this.changeMainPage("provinces");
  } 
  onUserLogged(name) {
      this.setState({
          currentUser: {
              isSignedIn: true,
              userName: name
          }
      });
  }
  onUserLogout() {
      this.setState({
          currentUser: {
              isSignedIn: false,
              userName: ""
          }
      });
  }
  render() {
      var mainPage = this.state.page == "main" ? (<HomeIndex />)
          : this.state.page == "login" ? (<LoginPage onUserLogged={this.onUserLogged} />)
              : (<h1>eshsrth</h1>);
      return <>
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
                                  <a className="nav-link text-dark" onClick={this.onHomeClick}>Главная</a>
                              </li>
                              <li className="nav-item">
                                  <a className="nav-link text-dark" onClick={this.onMyOrganizationClick}>Моя провинция</a>
                              </li>
                              <li className="nav-item">
                                  <a className="nav-link text-dark" onClick={this.onProvincesClick}>Провинции</a>
                              </li>
                          </ul>
                          <LoginPartial currentUser={this.state.currentUser} changeMainPage={this.changeMainPage} onUserLogout={this.onUserLogout}/>
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
      </>;
  }
}