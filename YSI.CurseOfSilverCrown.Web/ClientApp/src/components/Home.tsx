import * as React from 'react';
import Card, { ILink } from './Card';

import imgCardDefault from '../assets/img/cardDefault.jpg';
import imgMapDefault from '../assets/img/cardMap.jpg';
import imgHistoryDefault from '../assets/img/cardHistory.jpg';

const Home = () => {
  const renderWelcomeCard = () => {
    const loginLink: ILink = { name: 'Вход', url: '/Login' };
    const registerLink: ILink = { name: 'Регистрация', url: '/Register' };
    const title = 'Добро пожаловать в игру Проклятие Серебряной Короны!';
    return (
      <Card
        key={title}
        title={title}
        imgPath={imgCardDefault}
        isLeftSide={false}
        isSpecialOperation={false}
        links={[loginLink, registerLink]}
        text='Возьмите под управление один из регионов средневекового мира. 
          Развивайте свои земли, воюйте или договаривайтесь с соседями, заполучите вассалов и заслужите титул короля. 
          Войдите в свой аккаунт или пройдите регистрацию, чтобы присоединиться к игре.'
        time={undefined}
      />
    )
  }

  const renderMapCard = () => {
    const mapLink: ILink = { name: 'Карта', url: '/Map' };
    const title = 'Проработанная карта мира с множеством индивидуальных игровых регионов.';
    return (
      <Card
        key={title}
        title={title}
        imgPath={imgMapDefault}
        isLeftSide={true}
        isSpecialOperation={false}
        links={[mapLink]}
        text='На текущий момент в игре более сотни регионов. Сейчас они мало чем отличаются друг 
          от друга, но со временем каждый регион будет иметь индивидуальные черты.'
        time={undefined}
      />
    )
  }

  const renderHystoryCard = () => {
    const historyLink: ILink = { name: 'История', url: '/History' };
    const title = 'История мира на основе действий игроков.';
    return (
      <Card
        key={title}
        title={title}
        imgPath={imgHistoryDefault}
        isLeftSide={false}
        isSpecialOperation={false}
        links={[historyLink]}
        text='Возвышения королевств, мятежи вассалов, войны, постройки замков. 
          Игровые события сохраняются в истории мира и Вы можете внести свою главу в 
          развитии мира.'
        time={undefined}
      />
    )
  }

  return (
    <div className="text-center">
      {renderWelcomeCard()}
      {renderMapCard()}
      {renderHystoryCard()}
    </div>
  )
};

export default Home;
