export default function TurnEvents(props) {
  return props.events.length === 0 ? null : props.events.map((event, index) => 
    <p key={event[0].toString() + index} style={{marginBottom: 0}}>{event}</p>);
}
