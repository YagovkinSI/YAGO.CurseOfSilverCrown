import * as React from 'react';
import { CircleMarker, MapContainer, Marker, Popup, TileLayer, useMapEvents } from 'react-leaflet';
import { Button, Card, Typography } from '@mui/material';

import L from 'leaflet';
import markerIcon from 'leaflet/dist/images/marker-icon.png'
import markerShadow from 'leaflet/dist/images/marker-shadow.png'
import 'leaflet/dist/leaflet.css';
import './gameMap.css';
import type LinkOnClick from '../shared/LinkOnClick';

export interface GameMapProps {
    mapElements: GameMapElement[]
}

export interface GameMapElement {
    id: number,
    name: string,
    description: string,
    lat: number,
    lng: number,
    links: LinkOnClick[]
}

const GameMap: React.FC<GameMapProps> = ({ mapElements }) => {

    console.log(mapElements)

    const defaultZoom = 6
    const minZoom = 5
    const maxZoom = 6

    const [locationCoordinates, setLocationCoordinates] = React.useState<LatLng | null>(null);

    interface LatLng {
        lat: number;
        lng: number;
    }

    const defaultIcon = L.icon({
        iconUrl: markerIcon,
        shadowUrl: markerShadow,
        iconSize: [25, 41],
        iconAnchor: [12, 41],
        popupAnchor: [1, -34],
        shadowSize: [41, 41],
    });

    const LocationMarker: React.FC<{ setPosition: (position: LatLng) => void }> = ({ setPosition }) => {
        useMapEvents({
            click(e) {
                setPosition(e.latlng);
            },
        });

        return null;
    };

    const ZoomMap: React.FC = () => {
        const map = useMapEvents({
            zoomend() {
                let zoom = map.getZoom();
                if (zoom < minZoom) {
                    zoom = minZoom;
                    map.setZoom(zoom);
                }
                if (zoom > maxZoom) {
                    zoom = maxZoom;
                    map.setZoom(zoom);
                }
            },
        });
        return null;
    };

    const renderMapCoordinate = () => {
        const text = locationCoordinates == null
            ? 'Кликните на карту, чтобы узнать координаты локации'
            : `Координаты локации: ${locationCoordinates.lat.toFixed(3)} : ${locationCoordinates.lng.toFixed(3)}`;
        return (
            <Card style={{ height: 'var(--height-map-menu)', borderRadius: '0' }}>
                <Typography variant="subtitle2" gutterBottom style={{ textAlign: "center", fontWeight: "bold" }}>
                    {text}
                </Typography>
            </Card>
        )
    }

    const province = (id: number, position: LatLng, name: string, description: string) => {
        return (
            <CircleMarker key={id} center={position} pathOptions={{ color: 'green' }} radius={4}>
                <Popup>
                    <Typography gutterBottom>{name}</Typography>
                    <Typography gutterBottom color='var(--color-mutted)'>{description}</Typography>
                    <Button variant="contained" style={{ margin: "0 0.5rem" }}>Выбрать</Button>
                    <Button variant="outlined" style={{ margin: "0 0.5rem" }}>Подробнее...</Button>
                </Popup>
            </CircleMarker>
        )
    }

    const provinces = () => {
        return (<>
            {mapElements.map((element) => {
                return province(element.id, { lat: element.lat, lng: element.lng }, element.name, element.description)
            })}
        </>
        )
    }

    const renderMap = () => {
        return (
            <MapContainer
                center={[40, 20]}
                zoom={defaultZoom}
                scrollWheelZoom={true}
                maxBounds={[[20.8, 14], [48, 59]]}>
                <TileLayer
                    url="./images/worldmap/{z}-{x}-{y}.jpg"
                    noWrap
                />
                {provinces()}
                <LocationMarker setPosition={setLocationCoordinates} />
                {locationCoordinates &&
                    <Marker position={locationCoordinates} icon={defaultIcon} />
                }
                <ZoomMap />
            </MapContainer>
        )
    }

    const render = () => {
        return (
            <>
                {renderMap()}
                {renderMapCoordinate()}
            </>
        )
    }

    return render();
};

export default GameMap;