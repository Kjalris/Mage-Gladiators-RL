using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpellCasting : MonoBehaviour
{
    // the spell itself
    public GameObject spell;

    [Header("the force of the spell")]
    [SerializeField] float spell_force;
    [SerializeField] float upward_force;

    [Header("Spell stats")]
    [SerializeField] float time_between_spell_casting;
    [SerializeField] float spread_of_spell;
    [SerializeField] float mana_regain_time;
    [SerializeField] float time_between_spells;
    [SerializeField] float damage;

    [Header("Mana")]
    [SerializeField] int mana_size;
    [SerializeField] int mana_pr_spellcast;

    [Header("bools and stuff")]
    public bool allow_button_hold;
    private int mana_left, mana_used;
    public bool casting, ready_to_cast, reloading;
    public bool allow_invoke = true;

    public Camera cam;
    public Transform attack_point;
    public TextMeshProUGUI mana_display;
    public ManaBar manaBar;

    // player layer mask and value used to ignore spellcasting in the play layer (ray cast stuff)
    public LayerMask player_layer;
    int layerMask;

    // make sure when starting that mana is set to max and is ready
    public void Awake()
    {
        mana_left = mana_size;
        ready_to_cast = true;
    }

    public void Start()
    {
        mana_left = mana_size;
        manaBar.SetMaxMana(mana_size);
        // Ignore all layers except the layer which is specified. The ~ makes it inverse so it ignores the player layer but uses all the others
        layerMask = ~player_layer.value;
    }

    public void Update()
    {
        MyInput();

        if (mana_display != null)
        {
            mana_display.SetText(mana_left / mana_pr_spellcast + " / " + mana_size / mana_pr_spellcast);
        }
        manaBar.SetMana(mana_left);
    }

    private void MyInput()
    {
        if (allow_button_hold) casting = Input.GetKey(KeyCode.Mouse0);
        else casting = Input.GetKeyDown(KeyCode.Mouse0);

        // regain mana
        if (Input.GetKeyDown(KeyCode.R) && mana_left < mana_size && !reloading)
        {
            Reload();
        } 
        if (ready_to_cast && casting && !reloading && mana_left <= 0)
        {
            Reload();
        }
        // auto reload
        if (mana_left == 0)
        {
            Reload();
        }

        // check if all the parameters are ready to be able to cast
        if (ready_to_cast && casting && !reloading && mana_left > 0)
        {
            mana_used = 0;

            Cast();
        }
    }

    private void Cast()
    {
        ready_to_cast = false;

        // find the hit position using a raycast
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // check if ray hit anything
        Vector3 target_point;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            target_point = hit.point;
        }
        else
        {
            target_point = ray.GetPoint(100); // this point is just far away from player, like if you are aiming at the sky
        }

        // calculate direction without spread
        Vector3 direction_without_spread = target_point - attack_point.position;

        // calcute spread
        float x = Random.Range(-spread_of_spell, spread_of_spell);
        float y = Random.Range(-spread_of_spell, spread_of_spell);

        // calculate new direction with spread
        Vector3 direction_with_spread = direction_without_spread + new Vector3(x,y,0);

        // instantiate spell
        GameObject current_spell = Instantiate(spell, attack_point.position, Quaternion.identity);

        // rotate spell
        current_spell.transform.forward = direction_with_spread.normalized;

        // add force to spell
        current_spell.GetComponent<Rigidbody>().AddForce(direction_with_spread.normalized * spell_force, ForceMode.Impulse);
        current_spell.GetComponent<Rigidbody>().AddForce(cam.transform.up * upward_force, ForceMode.Impulse);

        // update counter
        mana_left--;
        mana_used++;

        // invoke reset_spell function (if not already invoked)
        if (allow_invoke)
        {
            Invoke("ResetSpell", time_between_spell_casting);
            allow_invoke = false;
        }

        if (mana_used < mana_pr_spellcast && mana_left > 0)
        {
            Invoke("Cast", time_between_spells);
        }
    }

    private void ResetSpell()
    {
        // allow spell casting again
        ready_to_cast = true;
        allow_invoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", mana_regain_time);
    }

    private void ReloadFinished()
    {
        mana_left = mana_size;
        reloading = false;
    }
}
