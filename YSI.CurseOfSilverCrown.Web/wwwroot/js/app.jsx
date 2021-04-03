class HomeIndex extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            turnName: "",
            lastEventStories: [[]]
        };
    }
    loadData() {
        var xhr = new XMLHttpRequest();
        xhr.open("get", "api/Home/Index", true);
        xhr.onload = function () {
            let data = JSON.parse(xhr.responseText);
            this.setState({
                turnName: data.turn,
                lastEventStories: data.lastEventStories
            });
        }.bind(this);
        xhr.send();
    }
    componentDidMount() {
        this.loadData();
    }
    render() {
        return <>
            <div className="text-center">
                <h1 className="display-4">Добро пожаловать</h1>

                <p>Узнай о том как <a href="https://vk.com/club189975977">разрабатывается игра</a>.</p>

                <h3>На дворе {this.state.turnName}</h3>

                {/*@{}*/}
                {/*        var user = UserManager.GetUserAsync(User).Result;*/}
                {/*        var isAdmin = user != null*/}
                {/*        ?UserManager.IsInRoleAsync(user, "Admin").Result*/}
                {/*        : false;*/}
                {/*        if (isAdmin)*/}
                {/*        {<a className="nav-link text-dark" asp-area="" asp-controller="Admin" asp-action="NextTurn">Закончить ход</a>}*/}
                {/*    }*/}
            </div>
            <div>
                <h4>
                    Важнейшие события прошлого
                </h4>
                {/*{*/}
                {/*    this.state.lastEventStories.map(function (eventStory) {*/}
                {/*        return <>*/}
                {/*            {*/}
                {/*                eventStory.map(function (line) {*/}
                {/*                    return*/}
                {/*                    <p style="margin-bottom: 0;">{line}</p>*/}
                {/*                })*/}
                {/*            }*/}
                {/*            <p style="margin-bottom: 0;" > ____________________</p>*/}
                {/*        </>*/}
                {/*    })*/}
                {/*}*/}
            </div>
        </>
    }
}

class LoginPage extends React.Component {
    constructor(props) {
        super(props);

        this.onSubmit = this.onSubmit.bind(this);
        this.checkUser = this.checkUser.bind(this);
    }
    onSubmit(e) {
        e.preventDefault();
        var userName = this.refs.nameInput.value;
        var userPassword = this.refs.passwordInput.value;
        if (!userName || !userPassword) {
            return;
        }
        this.checkUser(userName, userPassword);            
    }
    checkUser(name, password) {
        const data = new FormData();
        data.append("userName", name);
        data.append("password", password);
        data.append("rememberMe", false);
        var xhr = new XMLHttpRequest();

        xhr.open("post", "/api/Auth", true);
        xhr.onload = function () {
            if (xhr.status === 200) {
                this.props.onUserLogged(name);
            }
            else {
                alert("Неверный логин или пароль.");
            }
        }.bind(this);
        xhr.send(data);
    }
    render() {
        return <>
            <h1>Вход</h1>
            <div className="row">
                <div className="col-md-4">
                    <section>
                        <form onSubmit={this.onSubmit} >
                            <h4>Используйте локальную учетную запись для входа в систему.</h4>
                            <hr />
                            <div asp-validation-summary="All" className="text-danger"></div>
                            <div className="form-group">
                                <label>Логин</label>
                                <input className="form-control"
                                    type="text"
                                    ref="nameInput" />
                            </div>
                            <div className="form-group">
                                <label>Пароль</label>
                                <input className="form-control"
                                    type="password"
                                    ref="passwordInput" />
                            </div>
                            {/*<div className="form-group">*/}
                            {/*    <div className="checkbox">*/}
                            {/*        <label asp-for="Input.RememberMe">*/}
                            {/*            <input asp-for="Input.RememberMe" />*/}
                            {/*        @Html.DisplayNameFor(m => m.Input.RememberMe)*/}
                            {/*        </label>*/}
                            {/*    </div>*/}
                            {/*</div>*/}
                            <div className="form-group">
                                <button type="submit" className="btn btn-primary">Вход</button>
                            </div>
                            {/*<div className="form-group">*/}
                            {/*    <p>*/}
                            {/*        <a id="forgot-password" asp-page="./ForgotPassword">Забыли Ваш пароль?</a>*/}
                            {/*    </p>*/}
                            {/*    <p>*/}
                            {/*        <a>*/}{/*asp-page="./Register" asp-route-returnUrl="@Model.ReturnUrl"*/}
                            {/*            Зарегистрируйтесь как новый пользователь*/}
                            {/*        </a>*/}
                            {/*    </p>*/}
                            {/*    <p>*/}
                            {/*        <a id="resend-confirmation" asp-page="./ResendEmailConfirmation">Повторно отправить подтверждение по электронной почте</a>*/}
                            {/*    </p>*/}
                            {/*</div>*/}
                        </form>
                    </section>
                </div>
            </div>
        </>;
    }
}

class LoginPartial extends React.Component {   
    constructor(props) {
        super(props);
        this.onLoginClick = this.onLoginClick.bind(this);
        this.onLogoutClick = this.onLogoutClick.bind(this);
    }
    onLoginClick() {
        this.props.changeMainPage("login");
    }
    onLogoutClick() {
        this.props.onUserLogout();
    }
    render() {
        if (this.props.currentUser.isSignedIn) {
            return <ul className="navbar-nav">
                <li className="nav-item">
                    <a className="nav-link text-dark" > {/*asp-area="Identity" asp-page="/Account/Manage/Index" title="Manage"*/}
                        Здравствуйте, {this.props.currentUser.userName}!
                    </a>
                </li>
                <li className="nav-item">
                    <form className="form-inline">
                        <button className="nav-link btn btn-link text-dark" onClick={this.onLogoutClick}>Выйти</button>
                    </form>
                </li>
            </ul >
        } else {
            return <ul className="navbar-nav">                
                {/*<li className="nav-item">*/}
                {/*    <a className="nav-link text-dark" asp-area="Identity" asp-page="/Account/Register">Регистрация</a>*/}
                {/*</li>*/}
                <li className="nav-item">
                    <a className="nav-link text-dark" onClick={this.onLoginClick}>Вход</a>
                </li>
            </ul >;
        }        
    }
}

class Layout extends React.Component {
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

ReactDOM.render(
    <Layout />,
    document.getElementById("content")
);