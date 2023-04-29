import * as React from 'react';

import './Card.css';
import { Link } from 'react-router-dom';

export interface ILink {
    url: string,
    name: string
}

interface ICardProps {
    key: string,
    isLeftSide: boolean,
    imgPath: string,
    title: string,
    text: string,
    isSpecialOperation: boolean,
    time: string | undefined
    links: ILink[]
}

const Card: React.FC<ICardProps> = (props) => {
    const timeToNextTurn = () => {
        return (
            <p className="card-text" style={{ color: 'red' }}>
                Обработка приказов текущего хода будет выполняться
                <span id="time">{props.time}</span>.
            </p>
        )
    }

    const links = () => {
        return (
            <React.Fragment>
                {props.links.map((link) =>
                    <Link key={link.url} to={link.url} style={{ margin: '0 15px' }}> 
                        {link.name}
                    </Link>
                )}
            </React.Fragment>
        )
    }

    return (
        <div className="card">
            <img
                src={props.imgPath}
                className={props.isLeftSide ? "card-img-left" : "card-img-right"}
                alt="Картинка" />
            <div className="card-body">
                <h5 className="card-title">{props.title}</h5>
                {props.isSpecialOperation ? timeToNextTurn() : <></>}
                <p className="card-text">{props.text}</p>
                <div className="card-link">
                    {links()}
                </div >
            </div >
        </div >
    )
};

export default Card;
