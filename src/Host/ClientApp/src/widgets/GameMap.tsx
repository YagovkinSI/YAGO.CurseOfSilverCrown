import * as React from 'react';
import { CircleMarker, ImageOverlay, MapContainer, Marker, Popup, useMapEvents, GeoJSON } from 'react-leaflet';
import { Button, Card, Typography } from '@mui/material';

import L from 'leaflet';
import markerIcon from 'leaflet/dist/images/marker-icon.png'
import markerShadow from 'leaflet/dist/images/marker-shadow.png'
import 'leaflet/dist/leaflet.css';
import './gameMap.css';
import type LinkOnClick from '../shared/LinkOnClick';
import mapImage from '../assets/images/worldmap/map.jpg'
import mapData from '../assets/geoJson/mapGeoJson.json'
import type { FeatureCollection } from 'geojson';

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

    const FILL_COLOR_DEFAULT : string = "blue";
    const FILL_OPACITY_DEFAULT : number = 0.7;

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

    const onEachFeature = (_: any, layer: L.Layer) => {
        layer.on({
            mouseover: (e) => {
                const hoveredLayer = e.target;
                hoveredLayer.setStyle({
                    fillColor: "white",
                    fillOpacity: 0.8
                });
            },
            mouseout: (e) => {
                const hoveredLayer = e.target;
                hoveredLayer.setStyle({
                    fillColor: FILL_COLOR_DEFAULT,
                    fillOpacity: FILL_OPACITY_DEFAULT
                });
            }
        });
    };

    const renderMap = () => {
        const geoJsonData = mapData as FeatureCollection;

        return (
            <MapContainer
                crs={L.CRS.Simple}
                bounds={[[0, 0], [2076, 1839]]}
                minZoom={-2}
                zoom={0}
                scrollWheelZoom={true}>

                <ImageOverlay
                    url={mapImage}
                    bounds={[[0, 0], [2076, 1839]]}
                />

                <GeoJSON
                    data={geoJsonData}
                    style={{
                        fillColor: FILL_COLOR_DEFAULT,
                        weight: 2,
                        opacity: 1,
                        color: 'white',
                        fillOpacity: FILL_OPACITY_DEFAULT
                    }}
                    onEachFeature={onEachFeature}
                />

                {provinces()}
                <LocationMarker setPosition={setLocationCoordinates} />
                {locationCoordinates &&
                    <Marker position={locationCoordinates} icon={defaultIcon} />
                }
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