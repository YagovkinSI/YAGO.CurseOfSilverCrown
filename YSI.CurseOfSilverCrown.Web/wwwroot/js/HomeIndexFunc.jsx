import { useEffect, useState } from "react";

export default function HomeIndex () {
  const [turnName, setTurnName] = useState('');
  const [lastEventStories, setLastEventStories] = useState([[]]);

  useEffect(() => {
    var xhr = new XMLHttpRequest();
    xhr.open("get", "api/Home/Index", true);
    xhr.onload = () => {
        let data = JSON.parse(xhr.responseText);
        setTurnName(data.turn);
        setLastEventStories(data.lastEventStories);
    };
    xhr.send();
  }, []);

  return (
    <div>
      <div className="text-center">
        <h1 className="display-4">Добро пожаловать</h1>
        <p>Узнай о том как <a href="https://vk.com/club189975977">разрабатывается игра</a>.</p>
        <h3>На дворе {turnName}</h3>
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
    </div>
  );
}
